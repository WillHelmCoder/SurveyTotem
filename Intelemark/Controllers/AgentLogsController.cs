using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Models.Reports;
using Intelemark.Services;
using Microsoft.AspNet.Identity;

namespace Intelemark.Controllers
{
    [Authorize]
    public class AgentLogsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            try
            {
                String AgentId = (User.IsInRole("Admin") || User.IsInRole("Account Manager")) ? "-1" : User.Identity.GetUserId();

                //Admin or Account Manager
                if (User.IsInRole("Admin") || User.IsInRole("Account Manager"))
                {
                    var Users = db.Users.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3"));
                    ViewBag.AccountManagerId = new SelectList(Users, "Id", "DisplayName");
                    ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
                    var StartDate = DateTime.Now.AddDays(-7);
                    var EndDate = DateTime.Now.AddHours(23.99);
                    var newModel = new List<AgentLogModel>();
                    return View(newModel);
                }
                else
                {
                    var entities = db.AgentLogs.Include(a => a.Campaign).Include(a => a.Project).Include(a => a.TimeZone).Where(z => !z.IsDeleted && (z.AgentId == AgentId || AgentId == "-1")).ToList();
                    var models = new List<AgentLogModel>();
                    foreach (var entity in entities)
                        models.Add(new AgentLogModel
                        {
                            Id = entity.Id,
                            Agent = entity.Agent,
                            DialingHours = entity.DialingHours,
                            Successes = entity.Successes,
                            TrainingHours = entity.TrainingHours,
                            Campaign = entity.Campaign,
                            TimeZone = entity.TimeZone,
                            Project = entity.Project,
                            LastUpdate = entity.LastUpdate,
                            CreationDate = entity.CreationDate
                        });

                    return View(models);
                }


            }
            catch (Exception e)
            {
                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(AgentLogModel model)
        {
            try
            {
                //Admin or Account Manager
                if (User.IsInRole("Admin") || User.IsInRole("Account Manager"))
                {
                    var endDate = model.EndDate.AddHours(23.99);
                    var entities = db.AgentLogs.Include(a => a.Campaign).Include(a => a.Project).Include(a => a.TimeZone).Where(z => !z.IsDeleted && (z.AgentId == model.AccountManagerId) && z.CampaignId == model.CampaignId && (z.CreationDate <= endDate && z.CreationDate >= model.StartDate)).ToList();
                    var models = new List<AgentLogModel>();
                    foreach (var entity in entities)
                        models.Add(new AgentLogModel
                        {
                            Id = entity.Id,
                            Agent = entity.Agent,
                            DialingHours = entity.DialingHours,
                            Successes = entity.Successes,
                            TrainingHours = entity.TrainingHours,
                            Campaign = entity.Campaign,
                            TimeZone = entity.TimeZone,
                            Project = entity.Project,
                            LastUpdate = entity.LastUpdate,
                            CreationDate = entity.CreationDate,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate
                        });

                    var Users = db.Users.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3"));
                    ViewBag.AccountManagerId = new SelectList(Users, "Id", "DisplayName", model.AccountManagerId);
                    ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier", model.CampaignId);
                    return View(models);

                }
                else
                {
                    var entities = db.AgentLogs.Include(a => a.Campaign).Include(a => a.Project).Include(a => a.TimeZone).Where(z => !z.IsDeleted && (z.AgentId == User.Identity.GetUserId())).ToList();
                    var models = new List<AgentLogModel>();
                    foreach (var entity in entities)
                        models.Add(new AgentLogModel
                        {
                            Id = entity.Id,
                            Agent = entity.Agent,
                            DialingHours = entity.DialingHours,
                            Successes = entity.Successes,
                            TrainingHours = entity.TrainingHours,
                            Campaign = entity.Campaign,
                            TimeZone = entity.TimeZone,
                            Project = entity.Project,
                            LastUpdate = entity.LastUpdate,
                            CreationDate = entity.CreationDate
                        });

                    return View(models);
                }


            }
            catch (Exception e)
            {
                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View(new List<AgentLogModel>());
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted).Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
                ViewBag.ProjectId = new SelectList(db.Projects.Where(x => !x.IsDeleted), "Id", "Name");
                ViewBag.TimeZoneId = new SelectList(db.TimeZones, "Id", "Name");

                var AgentLogCount = db.AgentLogs.AsEnumerable().Where(x => x.AgentId == User.Identity.GetUserId() && x.CreationDate.Date == DateTime.Now.Date && !x.IsDeleted);
                if (AgentLogCount.Count() > 0)
                {
                    return RedirectToAction("Edit", new { id = AgentLogCount.FirstOrDefault().Id });
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }

            ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted).Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Projects.Where(x => !x.IsDeleted), "Id", "Name");
            ViewBag.TimeZoneId = new SelectList(db.TimeZones, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AgentLogModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.IsInRole("Admin"))
                    {
                        var entity = new AgentLog
                        {
                            DialingHours = model.DialingHours,
                            TrainingHours = model.TrainingHours,
                            Successes = model.Successes,
                            AgentId = model.UserId,
                            CampaignId = db.Projects.FirstOrDefault(x => x.Id == model.ProjectId).CampaignId,
                            ProjectId = model.ProjectId,
                            TimeZoneId = model.TimeZoneId,
                            LastUpdate = DateTime.Now,
                            CreationDate = model.Date,
                            IsDeleted = false,
                        };
                        db.AgentLogs.Add(entity);
                    }
                    else
                    {
                        var entity = new AgentLog
                        {
                            DialingHours = model.DialingHours,
                            TrainingHours = model.TrainingHours,
                            Successes = model.Successes,
                            AgentId = User.Identity.GetUserId(),
                            CampaignId = (Int32)Session["CampaignId"],
                            ProjectId = (Int32)Session["ProjectId"],
                            TimeZoneId = (Int32)Session["TimeZoneId"],
                            LastUpdate = DateTime.Now,
                            CreationDate = DateTime.Now,
                            IsDeleted = false,
                        };
                        db.AgentLogs.Add(entity);
                    }

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
                ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted).Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
                ViewBag.ProjectId = new SelectList(db.Projects.Where(x => !x.IsDeleted), "Id", "Name");
                ViewBag.TimeZoneId = new SelectList(db.TimeZones, "Id", "Name");

                return View();
            }

            ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted).Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Projects.Where(x => !x.IsDeleted), "Id", "Name");
            ViewBag.TimeZoneId = new SelectList(db.TimeZones, "Id", "Name");
            return View();
        }

        public async Task<ActionResult> Edit(Int32? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.AgentLogs.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new AgentLogModel
                {
                    DialingHours = entity.DialingHours,
                    TrainingHours = entity.TrainingHours,
                    Successes = entity.Successes,
                };

                return View(model);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                return View(new AgentLogModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AgentLogModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.AgentLogs.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.DialingHours = model.DialingHours;
                    entity.TrainingHours = model.TrainingHours;
                    entity.Successes = model.Successes;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit(model)", this.GetType().ToString(), AlertType.error, e);
                return View(model);
            }

        }


        public async Task<ActionResult> Delete(int id)
        {
            var res = await Service<AgentLog>.Delete(new AgentLog { Id = id });
            return RedirectToAction("Index");
        }


    }

}
