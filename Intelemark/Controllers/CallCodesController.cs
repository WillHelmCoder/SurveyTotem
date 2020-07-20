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

namespace Intelemark.Controllers
{
    [Authorize]
    public class CallCodesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                var output = await db.CallCodes
                    .Where(x => !x.IsDeleted)
                    .Select(x => new DTO.CallCodes.Output
                    {
                        CallCodeId = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Behavior = x.Behavior,
                        IsSuccess = x.IsSuccess,
                        
                        CampaignId = x.CampaignId,
                        CampaignDescription = x.Campaign.Description,
                        CampaignIdentifier = x.Campaign.Identifier,
                    })
                    .ToListAsync();

                var campaigns = await db.Campaigns
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();

                //campaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(campaigns, "Id", "Identifier");

                return View(output);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(int CampaignId)
        {
            try
            {
                var output = await db.CallCodes
                    .Where(x => !x.IsDeleted 
                        && x.CampaignId == CampaignId)
                    .Select(x => new DTO.CallCodes.Output
                    {
                        CallCodeId = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Behavior = x.Behavior,
                        IsSuccess = x.IsSuccess,

                        CampaignId = x.CampaignId,
                        CampaignIdentifier = x.Campaign.Identifier,
                        CampaignDescription = x.Campaign.Description,
                        
                    })
                    .ToListAsync();

                var campaigns = await db.Campaigns
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();

                ViewBag.CampaignId = new SelectList(campaigns, "Id", "Identifier");

                return View(output);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Index", this.GetType().ToString(), AlertType.error, e);
                return View();
            }
        }

        public ActionResult Create()
        {
            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CallCodeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var campaign = await db.Campaigns
                        .SingleOrDefaultAsync(x => !x.IsDeleted
                            && x.Id == model.CampaignId);

                    if (campaign == null)
                    {
                        AddAlert("Campaign not found", AlertType.error);
                        return RedirectToAction("Index");
                    }

                    var callCode = new CallCode(
                        campaign,
                        model.Name,
                        model.Code,
                        model.Behavior,
                        model.IsSuccess,
                        DateTime.Now);

                    db.CallCodes.Add(callCode);

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", GetType().ToString(), AlertType.error, e);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.CallCodes.FindAsync(id);

                if (entity == null)
                    return HttpNotFound();

                var model = new CallCodeModel
                {
                    Name = entity.Name,
                    Code = entity.Code,
                    Behavior = entity.Behavior,
                    IsSuccess = entity.IsSuccess
                };

                return View(model);

            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit", this.GetType().ToString(), AlertType.error, e);
                return View(new ProjectModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CallCodeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.CallCodes.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.Name = model.Name;
                    entity.Code = model.Code;
                    entity.Behavior = model.Behavior;
                    entity.IsSuccess = model.IsSuccess;
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

        public ActionResult ExportCallCodes()
        {
            try
            {
                ViewBag.CampaignId = db.Campaigns.Where(x => !x.IsDeleted).ToList();
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExportCallCodes(CallCodeExportViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exportingCampaign = await db.Campaigns
                        .SingleOrDefaultAsync(x => !x.IsDeleted
                            && x.Id == model.ExportId);

                    if (exportingCampaign == null)
                    {
                        AddAlert("Campaign not found", AlertType.error);
                        return RedirectToAction("Index");
                    }

                    var importingCampaign = await db.Campaigns
                        .SingleOrDefaultAsync(x => !x.IsDeleted
                            && x.Id == model.InNOutMonsterFries);

                    if (importingCampaign == null)
                    {
                        AddAlert("Campaign not found", AlertType.error);
                        return RedirectToAction("Index");
                    }

                    var callCodes = await db.CallCodes
                        .Where(x => !x.IsDeleted
                            && x.CampaignId == exportingCampaign.Id)
                        .ToListAsync();

                    var newCallCodes = new List<CallCode>();
                    foreach (var eoc in callCodes)
                    {
                        newCallCodes.Add(new CallCode(
                            importingCampaign,
                            eoc.Name,
                            eoc.Code,
                            eoc.Behavior,
                            eoc.IsSuccess,
                            DateTime.Now));

                        //db.CampaignCallCodes.Add(new CampaignCallCode { CampaignId = model.InNOutMonsterFries, CallCodeId = eoc.CallCodeId, IsDeleted = false, CreationDate = DateTime.Now, LastUpdate = DateTime.Now });
                    }

                    db.CallCodes.AddRange(newCallCodes);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            return RedirectToAction("Index");
        }


        public async Task<JsonResult> GetCallCodes(int CampaignId)
        {
            var output = new DataWrapper
            {
                data = await db.CallCodes
                    .Where(x => x.CampaignId == CampaignId)
                    .Select(x => new Wrapper
                    {
                        Name = x.Name,
                        Description = x.Code
                        
                    }).ToListAsync(),
            };

            return Json(output, JsonRequestBehavior.AllowGet);
        }




    }
}
