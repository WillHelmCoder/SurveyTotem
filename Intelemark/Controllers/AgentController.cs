using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Microsoft.AspNet.Identity;
using static ExtensionMethods.Extensions;
using System.Linq.Dynamic.Core;
using System.Transactions;
using Intelemark.Utilities;

namespace Intelemark.Controllers
{
    //[CheckSession]
    //[Authorize]
    public class AgentController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Account Manager, Agent")]
        public async Task<ActionResult> Index(bool? hasError)
        {
            try
            {
                var CampaignId = (int)Session["CampaignId"];

                var forms = await db.Forms
                    .Where(x => !x.IsDeleted
                        && x.CampaignId == CampaignId)
                    .ToListAsync();


                if (forms.Count < 1 && (hasError == false || hasError == null))
                {
                    if (forms.Count == 0)
                        return RedirectToAction("Module");

                    return RedirectToAction("Module", new { id = forms.FirstOrDefault().Id });
                }

                var output = new List<DTO.Forms.Output>();
                foreach (var entity in forms)
                {
                    output.Add(new DTO.Forms.Output
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        CampaignId = entity.CampaignId,
                        //Campaign = entity.Campaign,
                        //LastUpdate = entity.LastUpdate,
                        //CreationDate = entity.CreationDate,
                    });
                }

                ViewBag.HasError = hasError ?? false;

                return View(output);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        public async Task<ActionResult> Callbacks(bool? hasError)
        {
            try
            {
                var currentProject = (int)Session["ProjectId"];
                var myID = User.Identity.GetUserId();
                var currentTimezone = (int)Session["TimeZoneId"];
                var UTCNow = DateTime.Now.ToUniversalTime().AddMinutes(-10);
                var entities = await db.Appointments.Where(x => x.AgentId == myID && !x.IsDeleted && x.IsScheduled && x.ReminderDate >= UTCNow && x.Record.Contact.ProjectId == currentProject && x.Record.Contact.TimeZoneId == currentTimezone).ToListAsync();
                var models = new List<AppointmentModel>();
                foreach (var entity in entities)
                    models.Add(new AppointmentModel
                    {
                        Id = entity.Id,
                        Record = entity.Record,
                        Notes = entity.Notes,
                        DateScheduled = entity.DateScheduled,
                    });
                ViewBag.HasError = hasError ?? false;
                return View(models);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

    


        public async Task<ActionResult> AnswerForm(int? id)
        {
            try
            {
                var entity = id != null ?
                    await db.Forms.FindAsync(id) :
                    new Form();

                if (entity == null)
                    return HttpNotFound();

                              

                var model = new SingleFormModel
                {
                    Form = id != null ? new FormModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        CampaignId = entity.CampaignId,
                        Campaign = entity.Campaign,
                        QuestionList = entity.Questions.Select(y => new QuestionModel
                        {
                            Id = y.Id,
                            Question = y.Name,
                            TypeId = y.TypeId,
                            Order = y.Order,
                            Points = y.Points,
                            Image = y.Image,
                            AnswerList = y.Answers.Select(z => new AnswerModel
                            {
                                Id = z.Id,
                                Answer = z.Name,
                                Order = z.Order
                            }).ToList()
                        }).ToList()

                    } : null,
                   
                };               
                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Details", this.GetType().ToString(), AlertType.error, e);
                return View(new SingleFormModel());
            }
        }

        public static async Task<ContactModel> GetNext(CampaignModel campaign, int projectId, string myId, int TimeZoneId)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var priorityConfig = await db.ProjectPriorities.Where(x => x.ProjectId == projectId).OrderBy(p => p.PriorityValue).ToListAsync();
                        Contact entity = null;
                        ContactModel contact = null;

                        if (priorityConfig.Count > 0)
                        {
                            var Contacts = db.Contacts.Where(contacts => !contacts.IsDeleted && !contacts.IsFinalized && (contacts.AgentId == myId || contacts.AgentId == null) && contacts.ProjectId == projectId
                                                             && contacts.Attempts < campaign.MaxAttempt && contacts.TimeZoneId == TimeZoneId && !contacts.InHold && (!contacts.OnDial || contacts.AgentId.Equals(myId))).OrderBy(x => x.Attempts);

                            if (Contacts.Count() <= 0) return null;

                            var Attempts = Contacts.FirstOrDefault().Attempts;


                            var fst = priorityConfig.Where(x => x.PriorityValue == 1).First();
                            var snd = priorityConfig.Where(x => x.PriorityValue == 2).First();
                            var trd = priorityConfig.Where(x => x.PriorityValue == 3).First();

                            foreach (var priority in fst.ProjectPriorityDetails)
                            {
                                var query = $"{fst.Field}.Contains(@0) && Attempts <= (@1)";
                                var tempContacts = Contacts.Where(query, priority.FieldValue, Attempts);
                                if (tempContacts.Count() <= 0) continue;

                                foreach (var priority2 in snd.ProjectPriorityDetails)
                                {
                                    query = $"{snd.Field}.Contains(@0)";
                                    tempContacts = tempContacts.Where(query, priority2.FieldValue);
                                    if (tempContacts.Count() <= 0) continue;

                                    foreach (var priority3 in trd.ProjectPriorityDetails)
                                    {
                                        query = $"{trd.Field}.Contains(@0)";
                                        tempContacts = tempContacts.Where(query, priority3.FieldValue);
                                        if (tempContacts.Count() > 0) break;
                                    }
                                    if (tempContacts.Count() > 0) break;
                                }
                                entity = tempContacts.FirstOrDefault();
                                if (entity != null) break;
                            }

                            if (entity == null)
                            {
                                foreach (var priority2 in snd.ProjectPriorityDetails)
                                {
                                    var query = $"{snd.Field}.Contains(@0) && Attempts <= (@1)";
                                    var tempContacts = Contacts.Where(query, priority2.FieldValue, Attempts);
                                    if (tempContacts.Count() <= 0) continue;

                                    foreach (var priority3 in trd.ProjectPriorityDetails)
                                    {
                                        query = $"{trd.Field}.Contains(@0)";
                                        tempContacts = tempContacts.Where(query, priority3.FieldValue);
                                        if (tempContacts.Count() > 0) break;
                                    }
                                    entity = tempContacts.FirstOrDefault();
                                    if (entity != null) break;
                                }
                            }

                            if (entity == null)
                            {
                                foreach (var priority3 in trd.ProjectPriorityDetails)
                                {
                                    var query = $"{trd.Field}.Contains(@0)  && Attempts <= (@1)";
                                    var tempContacts = Contacts.Where(query, priority3.FieldValue, Attempts);
                                    entity = tempContacts.FirstOrDefault();
                                    if (entity != null) break;
                                }
                            }

                            if (entity == null)
                            {
                                contact = await GetNextNoPriority(campaign, projectId, myId, TimeZoneId, db);
                                transaction.Complete();
                                return contact;
                            }

                            entity.OnDial = true;
                            entity.AgentId = myId;

                            db.Entry(entity).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            contact = new ContactModel
                            {
                                Id = entity.Id,
                                Name = entity.Name,
                                PhoneNumber = entity.PhoneNumber,
                                AltPhoneNumber = entity.AltPhoneNumber,
                                Email = entity.Email,
                                Address = entity.Address,
                                City = entity.City,
                                Notes = entity.Notes,
                                State = entity.State,
                                Country = entity.Country,
                                ZipCode = entity.ZipCode,
                                ProjectId = entity.ProjectId,
                                TimeZoneId = entity.TimeZoneId,
                                Title = entity.Title,
                                Company = entity.Company,
                                AltAddress = entity.AltAddress,
                                Extension = entity.Extension
                            };

                        }
                        else
                        {
                            contact = await GetNextNoPriority(campaign, projectId, myId, TimeZoneId, db);
                        }

                        transaction.Complete();
                        return contact;

                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException TAE)
            {
                BaseController.LogError(TAE.Message, "AgentController - GetNext", TAE);
                return await GetNext(campaign, projectId, myId, TimeZoneId);
            }
            catch (Exception ex)
            {
                BaseController.LogError(ex.Message, "AgentController - GetNext", ex);
                return null;
            }
        }

        public static async Task<ContactModel> GetNextNoPriority(CampaignModel campaign, int projectId, string myId, int TimeZoneId, ApplicationDbContext db)
        {
            var c = await (from contacts in db.Contacts
                           where !contacts.IsDeleted && !contacts.IsFinalized && (contacts.AgentId == myId || contacts.AgentId == null) && contacts.ProjectId == projectId
                           && contacts.Attempts < campaign.MaxAttempt && contacts.TimeZoneId == TimeZoneId && !contacts.InHold && (!contacts.OnDial || contacts.AgentId.Equals(myId))
                           orderby contacts.Attempts
                           select contacts).ToListAsync();

            var contactEntity = c.FirstOrDefault();
            contactEntity.OnDial = true;
            contactEntity.AgentId = myId;
            db.Entry(contactEntity).State = EntityState.Modified;
            await db.SaveChangesAsync();

            if (c.Count() <= 0)
            {
                //dbContextTransaction.Rollback();
                return null;
            }

            return c.Select(x => new ContactModel
            {
                Id = x.Id,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                AltPhoneNumber = x.AltPhoneNumber,
                Email = x.Email,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Country = x.Country,
                ZipCode = x.ZipCode,
                Notes = x.Notes,
                ProjectId = x.ProjectId,
                TimeZoneId = x.TimeZoneId,
                Title = x.Title,
                Company = x.Company,
                AltAddress = x.AltAddress,
                Extension = x.Extension
            }).FirstOrDefault();
        }

        public static async Task<Contact> GetNextEntity(CampaignModel campaign, int projectId, string myId, int TimeZoneId, ApplicationDbContext db)
        {
            var c = await (from contacts in db.Contacts
                           where !contacts.IsDeleted && !contacts.IsFinalized && (contacts.AgentId == myId || contacts.AgentId == null) && contacts.ProjectId == projectId
                           && contacts.Attempts < campaign.MaxAttempt && contacts.TimeZoneId == TimeZoneId
                           orderby contacts.Attempts
                           select contacts).ToListAsync();

            if (c.Count <= 0)
            {
                throw new Exception();
            }

            return c.FirstOrDefault();
        }

        public async Task<ContactModel> GetContact(int ContactId)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        ContactModel contact;
                        contact = await db.Contacts.Where(x => x.Id == ContactId).Select(x => new ContactModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            PhoneNumber = x.PhoneNumber,
                            AltPhoneNumber = x.AltPhoneNumber,
                            Email = x.Email,
                            Address = x.Address,
                            City = x.City,
                            State = x.State,
                            Country = x.Country,
                            Notes = x.Notes,
                            ZipCode = x.ZipCode,
                            ProjectId = x.ProjectId,
                            TimeZoneId = x.TimeZoneId,
                            Title = x.Title,
                            Company = x.Company,
                            AltAddress = x.AltAddress,
                            Extension = x.Extension
                        }).FirstOrDefaultAsync();
                        transaction.Complete();
                        return contact;
                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException TAE)
            {
                BaseController.LogError(TAE.Message, "AgentController - GetNext", TAE);
                return await GetContact(ContactId);
            }
            catch (Exception ex)
            {
                BaseController.LogError(ex.Message, "AgentController - GetNext", ex);
                return null;
            }
        }





        [HttpPost]
        public async Task<ActionResult> SaveAnswerForm(AgentModel model)
        {
            try
            {
                

               
                var answeredForms = new List<AnsweredForm>();

                foreach (var item in model.Record.FormAnswers ?? new List<AnsweredFormModel>())
                {
                    if (item.CheckBoxAnswer != null)
                    {
                        foreach (var answer in item.CheckBoxAnswer.Where(x => x.Checked))
                        {
                            answeredForms.Add(new AnsweredForm
                            {
                                RecordId = 1,
                                Answer = answer.Answer,
                                QuestionId = item.QuestionId,
                                
                                CreationDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                IsDeleted = false
                            });
                        }
                    }
                    else if (!String.IsNullOrEmpty(item.Answer))
                    {
                        answeredForms.Add(new AnsweredForm
                        {
                            RecordId = 1,
                            Answer = item.Answer,
                            QuestionId = item.QuestionId,
                           
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            IsDeleted = false
                        });

                    }
                }
                                
                
                if (answeredForms.Count > 0) db.AnsweredForms.AddRange(answeredForms);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "SaveForm", this.GetType().ToString(), AlertType.error, e);
            }

            if (model.Record.FormId > 0)
                return RedirectToAction("AnswerForm", new { id = model.Record.FormId });
            else
                return RedirectToAction("AnswerForm");

        }




        [HttpPost]
        public async Task<ActionResult> SaveForm(AgentModel model)
        {
            try
            {
                var recordEntity = new Record()
                {
                    CallCodeId = model.Record.CallCodeId,
                    FormId = model.Record.FormId == 0 ? null : model.Record?.FormId,
                    ContactId = model.Record.ContactId,
                    UserId = User.Identity.GetUserId(),
                    EndTime = DateTime.Now,
                    StartTime = DateTime.Now.AddSeconds(-model.Record.Seconds),
                    LastUpdate = DateTime.Now,
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    Duration = model.Record.Seconds
                };
                db.Records.Add(recordEntity);
                
                var EOC = db.CallCodes
                    .FirstOrDefault(x => x.Id == model.Record.CallCodeId
                        && (x.Behavior == EOCBehaviors.CallBack
                        || x.Behavior == EOCBehaviors.SetUpAppointment));

                if (model.Appointment?.Agent != null && EOC != null)
                {
                    var TimeZoneId = (int)Session["TimeZoneId"];
                    var AppointmentEntity = new Appointments
                    {
                        AgentId = User.Identity.GetUserId(),
                        DateScheduled = model.Appointment.DateScheduled,
                        CampaignId = (int)Session["CampaignId"],
                        RecordId = recordEntity.Id,
                        IsConfirmed = false,
                        IsScheduled = EOC.Behavior == EOCBehaviors.CallBack,
                        Notes = model.Appointment.Notes,
                        IsDeleted = false,
                        CreationDate = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        ReminderDate = await ToUTC(model.Appointment.DateScheduled.AddMinutes(-10), TimeZoneId)
                    };

                    db.Appointments.Add(AppointmentEntity);
                }

                var answeredForms = new List<AnsweredForm>();

                foreach (var item in model.Record.FormAnswers ?? new List<AnsweredFormModel>())
                {
                    if (item.CheckBoxAnswer != null)
                    {
                        foreach (var answer in item.CheckBoxAnswer.Where(x => x.Checked))
                        {
                            answeredForms.Add(new AnsweredForm
                            {
                                RecordId = recordEntity.Id,
                                Answer = answer.Answer,
                                QuestionId = item.QuestionId,
                                
                                CreationDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                IsDeleted = false
                            });
                        }
                    }
                    else if (!String.IsNullOrEmpty(item.Answer))
                    {
                        answeredForms.Add(new AnsweredForm
                        {
                            RecordId = recordEntity.Id,
                            Answer = item.Answer,
                            QuestionId = item.QuestionId,
                            
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            IsDeleted = false
                        });

                    }
                }

                var entity = await db.Contacts.FindAsync(model.Record.ContactId);
                entity.Name = model.CurrentContact.Name;
                entity.PhoneNumber = model.CurrentContact.PhoneNumber;
                entity.AltPhoneNumber = model.CurrentContact.AltPhoneNumber;
                entity.Email = model.CurrentContact.Email;
                entity.Address = model.CurrentContact.Address;
                entity.City = model.CurrentContact.City;
                entity.State = model.CurrentContact.State;
                entity.Country = model.CurrentContact.Country;
                entity.ZipCode = model.CurrentContact.ZipCode;
                entity.Notes = model.CurrentContact.Notes;
                entity.LastUpdate = DateTime.Now;
                entity.Extension = model.CurrentContact.Extension;
                entity.Title = model.CurrentContact.Title;
                entity.Company = model.CurrentContact.Company;
                entity.AltAddress = model.CurrentContact.AltAddress;
                entity.Attempts += 1;
                entity.OnDial = false;
                entity.AgentId = User.Identity.GetUserId();

                var callCode = await db.CallCodes
                    .Where(x => x.Id == model.Record.CallCodeId)
                    .FirstOrDefaultAsync();

                if (callCode.IsSuccess || callCode.Behavior != EOCBehaviors.None)
                {
                    entity.IsFinalized = callCode.Behavior == EOCBehaviors.FinalizeRecord;
                    entity.AgentId = User.Identity.GetUserId();
                }

                if (entity.Project.Campaign.CompanyLink && callCode.Behavior != EOCBehaviors.None)
                {
                    var company = await db.Contacts.Where(x => x.Company.ToLower().Equals(entity.Company.ToLower())).ToListAsync();
                    foreach (var item in company)
                    {
                        item.IsFinalized = callCode.Behavior == EOCBehaviors.FinalizeCompany;
                        item.AgentId = User.Identity.GetUserId();
                        db.Entry(item).State = EntityState.Modified;
                    }
                }


                if (model.CallbackId > 0)
                {
                    await Services.Service<Appointments>.Delete(new Appointments { Id = model.CallbackId });
                }

               // var toAdd = new List<ExtraFieldValue>();

                foreach (var item in model.CurrentContact.ExtraFieldValues ?? new List<ExtraFieldValueModel>())
                {
                    if (item.Id > 0)
                    {
                      //  var extraFieldEntity = await db.ExtraFieldValues.FindAsync(item.Id);
                       // extraFieldEntity.Value = item.Value;
                        //extraFieldEntity.LastUpdate = DateTime.Now;
                        //db.Entry(extraFieldEntity).State = EntityState.Modified;
                    }
                    else
                    {
                        //toAdd.Add(new ExtraFieldValue
                        //{
                        //    ContactId = entity.Id,
                        //    ExtraFieldId = item.ExtraFieldId,
                        //    Value = item.Value,
                        //    LastUpdate = DateTime.Now,
                        //    CreationDate = DateTime.Now,
                        //    IsDeleted = false
                        //});
                    }
                }

                //if (toAdd.Count > 0)
                //    db.ExtraFieldValues.AddRange(toAdd);

                db.Entry(entity).State = EntityState.Modified;
                if (answeredForms.Count > 0) db.AnsweredForms.AddRange(answeredForms);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "SaveForm", this.GetType().ToString(), AlertType.error, e);
            }

            if (model.Record.FormId > 0)
                return RedirectToAction("Module", new { id = model.Record.FormId });
            else
                return RedirectToAction("Module");

        }

        public async Task<ActionResult> AgentSummary()
        {
            try
            {
                var onlineEmails = NotificationHub.OnlineAgents();
                var KeyValues = NotificationHub.GetConnections();
                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                var entities = await db.Users.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId.Equals("2") || y.RoleId.Equals("3"))).ToListAsync();
                var onlineUsers = onlineEmails.Select(c => entities.Where(x => x.Email.Equals(c)).FirstOrDefault()).Where(z => z != null).ToList();


                var onlineModel = KeyValues.Select(c => new OnlineUser
                {
                    User = entities.FirstOrDefault(x => x.Email.Equals(c.UserName)),
                    Campaign = Campaigns.FirstOrDefault(x => x.Id == c.CampaignId),
                    ConnectionTime = (DateTime.Now - db.UsersHistory.FirstOrDefault(x => x.Id == c.UserHistoryId)?.LoggedIn) ?? TimeSpan.FromSeconds(0)
                }
                ).Where(z => z != null).ToList();


                var offlineUsers = entities.Except(onlineUsers).ToList();

                var model = new UserHistoryModel
                {
                    OnlineUsers = onlineModel.Where(x => x.User != null).ToList(),
                    OfflineUsers = offlineUsers
                };

                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }


        }

        public async Task<DateTime> ToUTC(DateTime oldDate, int TimeZoneId)
        {
            var ModelTZ = await db.TimeZones.Where(x => !x.IsDeleted && x.Id == TimeZoneId).FirstOrDefaultAsync();
            return oldDate.AddHours(ModelTZ.CurrentTime);
        }


    }
}
