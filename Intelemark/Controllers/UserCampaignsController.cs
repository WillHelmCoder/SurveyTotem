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
using Intelemark.Services;
using Microsoft.AspNet.Identity;

namespace Intelemark.Controllers
{
    [Authorize]
    public class UserCampaignsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                List<Campaign> campaignSelectList = null;

                if (User.IsInRole("Admin"))
                {
                    campaignSelectList = await db.Campaigns
                        .Where(x => !x.IsDeleted)
                        .ToListAsync();
                }
                else if (User.IsInRole("Account Manager"))
                {
                    var campaigns = await db.UserCampaigns
                        .Where(x => !x.IsDeleted)
                        .ToListAsync();

                    campaignSelectList = campaigns
                        .Where(x => x.UserId == User.Identity.GetUserId())
                        .GroupBy(x => x.Campaign)
                        .Select(x => x.Key)
                        .ToList();
                }

                var userCampaigns = await db.UserCampaigns
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();

                var models = new List<UserCampaignModel>();
                foreach (var entity in userCampaigns)
                {
                    models.Add(new UserCampaignModel
                    {
                        Id = entity.Id,
                        User = entity.User,
                        UserId = entity.UserId,
                        Campaign = entity.Campaign,
                        CampaignId = entity.CampaignId,
                        BudgetedHours = entity.BudgetedHours,
                        PayRateDialingHours = entity.PayRateDialingHours,
                        PayRateSuccess = entity.PayRateSuccess,
                        PayRateTrainingHours = entity.PayRateTrainingHours,
                        LastUpdate = entity.LastUpdate,
                        CreationDate = entity.CreationDate,
                    });
                }

                ViewBag.CampaignId = new SelectList(campaignSelectList, "Id", "Identifier");

                return View(models);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", GetType().ToString(), AlertType.error, e);
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> Index(int CampaignId)
        {
            try
            {
                var entities = await db.UserCampaigns.Where(x => !x.IsDeleted && x.CampaignId == CampaignId).ToListAsync();
                var models = new List<UserCampaignModel>();

                List<Campaign> CampaignsSelectList = null;

                if (User.IsInRole("Admin"))
                {
                    CampaignsSelectList = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                    CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                }
                else if (User.IsInRole("Account Manager"))
                {
                    var Campaigns = await db.UserCampaigns.Where(x => !x.IsDeleted).ToListAsync();
                    CampaignsSelectList = Campaigns.Where(x => x.UserId == User.Identity.GetUserId()).GroupBy(x => x.Campaign).Select(x => x.Key).ToList();
                    CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                }

                foreach (var entity in entities)
                    models.Add(new UserCampaignModel
                    {
                        Id = entity.Id,
                        User = entity.User,
                        UserId = entity.UserId,
                        Campaign = entity.Campaign,
                        CampaignId = entity.CampaignId,
                        BudgetedHours = entity.BudgetedHours,
                        PayRateDialingHours = entity.PayRateDialingHours,
                        PayRateSuccess = entity.PayRateSuccess,
                        PayRateTrainingHours = entity.PayRateTrainingHours,
                        LastUpdate = entity.LastUpdate,
                        CreationDate = entity.CreationDate,
                    });

                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(models);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }

        }

        public ActionResult Create()
        {
            try
            {
                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.UserId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(UserCampaignModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var overWrite = db.UserCampaigns.Where(x => x.UserId == model.UserId && x.CampaignId == model.CampaignId).ToList();
                    if (overWrite.Count > 0 && model.confirmOverwite != 1)
                    {
                        Response.StatusCode = 400;
                        return Json("Error");
                    }

                    var userCampaign = db.UserCampaigns.Where(x => x.CampaignId == model.CampaignId && x.UserId == model.UserId).FirstOrDefault();
                    if (userCampaign != null)
                    {
                        db.UserProjects.RemoveRange(db.UserProjects.Where(y => y.UserCampaignId == userCampaign.Id && y.UserId == model.UserId));
                        db.UserCampaigns.RemoveRange(db.UserCampaigns.Where(x => x.UserId == model.UserId && x.CampaignId == model.CampaignId));
                    }

                    var entity = new UserCampaign
                    {
                        User = model.User,
                        UserId = model.UserId,
                        Campaign = model.Campaign,
                        CampaignId = model.CampaignId,
                        BudgetedHours = model.BudgetedHours,
                        PayRateDialingHours = model.PayRateDialingHours,
                        PayRateTrainingHours = model.PayRateTrainingHours,
                        PayRateSuccess = model.PayRateSuccess,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    db.UserCampaigns.Add(entity);

                    var userProjects = new List<UserProject>();

                    foreach (var item in model.ProjectIds)
                    {
                        userProjects.Add(new UserProject
                        {
                            UserCampaignId = entity.Id,
                            UserId = model.UserId,
                            ProjectId = item,
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            IsDeleted = false
                        });
                    }

                    db.UserProjects.AddRange(userProjects);
                    await db.SaveChangesAsync();

                    return Json("Ok");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            var Users = db.Users.Where(x => !x.IsDeleted);
            ViewBag.UserId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return Json("Error");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.UserCampaigns.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new UserCampaignModel
                {
                    User = entity.User,
                    UserId = entity.UserId,
                    Campaign = entity.Campaign,
                    CampaignId = entity.CampaignId,
                    BudgetedHours = entity.BudgetedHours,
                    PayRateDialingHours = entity.PayRateDialingHours,
                    PayRateSuccess = entity.PayRateSuccess,
                    PayRateTrainingHours = entity.PayRateTrainingHours,
                    UserProjectsList = entity.UserProjectList.Select(x => new UserProjectModel
                    {
                        ProjectId = x.ProjectId,
                        Project = x.Project,
                        UserId = x.UserId,
                    }).ToList()
                };

                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.UserId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3")), "Id", "DisplayName");
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
                return View(model);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                return View(new UserCampaignModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserCampaignModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.UserCampaigns.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.User = model.User;
                    entity.UserId = model.UserId;
                    entity.CampaignId = model.CampaignId;
                    entity.Campaign = model.Campaign;
                    entity.BudgetedHours = model.BudgetedHours;
                    entity.PayRateDialingHours = model.PayRateDialingHours;
                    entity.PayRateSuccess = model.PayRateSuccess;
                    entity.PayRateTrainingHours = model.PayRateTrainingHours;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.UserId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")), "Id", "DisplayName");
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
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
            var res = await Service<UserCampaign>.Delete(new UserCampaign { Id = id });
            return RedirectToAction("Index");
        }



        public JsonResult GetProjects(Int32 CampaignId)
        {
            var ProjectList = new DataWrapper();
            ProjectList.data = db.Projects.Where(x => x.CampaignId == CampaignId).Select(x => new Wrapper { Description = x.Description, Name = x.Name, Id = x.Id }).ToList();
            return Json(ProjectList, JsonRequestBehavior.AllowGet);
        }

    }

    public class Wrapper
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }

    public class DataWrapper
    {
        public List<Wrapper> data { get; set; }
    }
}
