using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Services;

namespace Intelemark.Controllers
{
    [Authorize]
    public class CampaignsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                var entities = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                var models = new List<CampaignModel>();
                foreach (var entity in entities)
                    models.Add(new CampaignModel
                    {
                        Id = entity.Id,
                        Identifier = entity.Identifier,
                        AccountManagerId = entity.AccountManagerId,
                        ClientId = entity.ClientId,
                        ActiveControl = entity.ActiveControl,
                        BillingPH = entity.BillingPH,
                        CompanyLink = entity.CompanyLink,
                        Objective = entity.Objective,
                        Description = entity.Description,
                        LastUpdate = entity.LastUpdate,
                        CreationDate = entity.CreationDate,
                        MaxAttempt = entity.MaxAttempt,
                        SpellCheck = entity.SpellCheck,
                        CampaignLimit = entity.CampaignLimit
                    });
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
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.ClientId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "1")), "Id", "DisplayName");
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CampaignModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Campaign
                    {
                        Identifier = model.Identifier,
                        AccountManagerId = model.AccountManagerId,
                        ActiveControl = model.ActiveControl,
                        BillingPH = model.BillingPH,
                        ClientId = model.ClientId,
                        CompanyLink = model.CompanyLink,
                        Objective = model.Objective,
                        Description = model.Description,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                        MaxAttempt = model.MaxAttempt,
                        SpellCheck = model.SpellCheck,
                        CampaignLimit = model.CampaignLimit,
                    };

                    var savedEntity = db.Campaigns.Add(entity);
                    await db.SaveChangesAsync();

                    foreach (var file in model.Files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            bool exists = System.IO.Directory.Exists(Server.MapPath($"~/Content/Campaigns/{savedEntity.Id}"));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath($"~/Content/Campaigns/{savedEntity.Id}"));

                            var path = Path.Combine(Server.MapPath($"~/Content/Campaigns/{savedEntity.Id}/"), fileName);
                            file.SaveAs(path);
                        }
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            var Users = db.Users.Where(x => !x.IsDeleted);
            ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
            ViewBag.ClientId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "1")), "Id", "DisplayName");
            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var entity = await db.Campaigns.FindAsync(id);
                List<StoredFileModel> files = new List<StoredFileModel>();

                if (entity == null)
                    return HttpNotFound();

                //Revisamos si hay archivos
                var path = Server.MapPath($"/Content/Campaigns/{entity.Id}");
                bool exists = System.IO.Directory.Exists(path);
                if (exists)
                {
                    foreach (var file in Directory.GetFiles(path).ToList())
                        files.Add(new StoredFileModel { FileUrl = $"/Content/Campaigns/{entity.Id}/{Path.GetFileName(file)}", FileName = Path.GetFileName(file) });
                }

                var model = new CampaignModel
                {
                    Identifier = entity.Identifier,
                    AccountManager = entity.AccountManager,
                    ActiveControl = entity.ActiveControl,
                    BillingPH = entity.BillingPH,
                    Client = entity.Client,
                    CompanyLink = entity.CompanyLink,
                    Objective = entity.Objective,
                    Description = entity.Description,
                    SpellCheck = entity.SpellCheck,
                    MaxAttempt = entity.MaxAttempt,
                    AccountManagerId = entity.AccountManagerId,
                    ClientId = entity.ClientId,
                    StoredFiles = files, 
                    CampaignLimit = entity.CampaignLimit
                };

                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.ClientId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "1")), "Id", "DisplayName");
                return View(model);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.ClientId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "1")), "Id", "DisplayName");
                return View(new CampaignModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CampaignModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Campaigns.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.Identifier = model.Identifier;
                    entity.AccountManagerId = model.AccountManagerId;
                    entity.ActiveControl = model.ActiveControl;
                    entity.BillingPH = model.BillingPH;
                    entity.ClientId = model.ClientId;
                    entity.CompanyLink = model.CompanyLink;
                    entity.Objective = model.Objective;
                    entity.Description = model.Description;
                    entity.SpellCheck = model.SpellCheck;
                    entity.MaxAttempt = model.MaxAttempt;
                    entity.CampaignLimit = model.CampaignLimit;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                var Users = db.Users.Where(x => !x.IsDeleted);
                ViewBag.AccountManagerId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "3" || y.RoleId == "4")), "Id", "DisplayName");
                ViewBag.ClientId = new SelectList(Users.Where(x => x.Roles.Any(y => y.RoleId == "1")), "Id", "DisplayName");
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
            var res = await Service<Campaign>.Delete(new Campaign { Id = id });
            return RedirectToAction("Index");
        }
    }
}
