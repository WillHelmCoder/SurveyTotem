using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Services;

namespace Intelemark.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {
                var entities = await db.Projects.Where(x => !x.IsDeleted).ToListAsync();
                var models = new List<ProjectModel>();
                foreach (var entity in entities)
                    models.Add(new ProjectModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        CampaignId = entity.CampaignId,
                        Description = entity.Description,
                        Priority = entity.Priority,
                        LastUpdate = entity.LastUpdate,
                        Campaign = entity.Campaign,
                        CreationDate = entity.CreationDate
                    });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");

                return View(models);
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
                var entities = await db.Projects.Where(x => !x.IsDeleted && x.CampaignId == CampaignId).ToListAsync();
                var models = new List<ProjectModel>();
                foreach (var entity in entities)
                    models.Add(new ProjectModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        CampaignId = entity.CampaignId,
                        Description = entity.Description,
                        Priority = entity.Priority,
                        LastUpdate = entity.LastUpdate,
                        Campaign = entity.Campaign,
                        CreationDate = entity.CreationDate
                    });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
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
                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Project
                    {
                        Name = model.Name,
                        CampaignId = model.CampaignId,
                        Description = model.Description,
                        Priority = model.Priority,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    db.Projects.Add(entity);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.Projects.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new ProjectModel
                {
                    Name = entity.Name,
                    Campaign = entity.Campaign,
                    Description = entity.Description,
                    CampaignId = entity.CampaignId,
                    Priority = entity.Priority
                };

                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier", model.CampaignId);
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
        public async Task<ActionResult> Edit(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Projects.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.Name = model.Name;
                    entity.CampaignId = model.CampaignId;
                    entity.Description = model.Description;
                    entity.Priority = model.Priority;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier", model.CampaignId);
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
            var res = await Service<Project>.Delete(new Project { Id = id });
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ProjectSettings(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var entity = await db.Projects.FindAsync(id);

            if (entity == null)
                return RedirectToAction("Index");

            var projSettings = new ProjectSettingsViewModel
            {
                Name = entity.Name,
                Id = entity.Id,
                Description = entity.Description,
            };

            projSettings.ProjectPriorities = await db.ProjectPriorities.OrderBy(y => y.PriorityValue).Where(x => x.ProjectId == id).Select(x =>
                new ProjectPriorityModel
                {
                    Field = x.Field,
                    PriorityValue = x.PriorityValue,
                    ProjectPriorityDetails = x.ProjectPriorityDetails.Select(z =>
                                             new ProjectPriorityDetailModel
                                             {
                                                 FieldValue = z.FieldValue,
                                                 FieldPriorityValue = z.FieldPriorityValue
                                             }
                   ).OrderBy(z => z.FieldPriorityValue).ToList(),
                    Id = x.Id,
                    ProjectId = x.ProjectId
                }
           ).ToListAsync();


            if (projSettings.ProjectPriorities.Count <= 0)
            {
                projSettings.ProjectPriorities.Add(new ProjectPriorityModel { Field = "Title", PriorityValue = 1, ProjectPriorityDetails = new List<ProjectPriorityDetailModel>() });
                projSettings.ProjectPriorities.Add(new ProjectPriorityModel { Field = "Company", PriorityValue = 2, ProjectPriorityDetails = new List<ProjectPriorityDetailModel>() });
                projSettings.ProjectPriorities.Add(new ProjectPriorityModel { Field = "SIC", PriorityValue = 3, ProjectPriorityDetails = new List<ProjectPriorityDetailModel>() });

            }


            return View(projSettings);
        }

        [HttpPost]
        public async Task<JsonResult> ProjectSettings(ProjectSettingsViewModel model)
        {
            if (model == null)
                return Json("Error");
            bool error = false;
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ProjectPriorities != null)
                    {
                        db.ProjectPriorityDetails.RemoveRange(db.ProjectPriorityDetails.Where(x => x.ProjectPriority.ProjectId == model.Id));
                        db.ProjectPriorities.RemoveRange(db.ProjectPriorities.Where(x => x.ProjectId == model.Id));

                        var projectPriorities = model.ProjectPriorities.Select(c => new ProjectPriority
                        {
                            ProjectId = model.Id,
                            Field = c.Field,
                            PriorityValue = c.PriorityValue,
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                        }).ToList();
                        var savedEntities = db.ProjectPriorities.AddRange(projectPriorities);

                        await db.SaveChangesAsync();

                        if (model.ProjectPriorityDetails != null)
                        {
                            db.ProjectPriorityDetails.AddRange(model.ProjectPriorityDetails.Select(c => new ProjectPriorityDetail
                            {
                                ProjectPriorityId = savedEntities.Where(x => x.Field.Equals(c.Type)).FirstOrDefault().Id,
                                FieldValue = c.FieldValue.Equals("Blank") ? string.Empty : c.FieldValue,
                                FieldPriorityValue = c.FieldPriorityValue,
                                CreationDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                            }).AsEnumerable());
                        }
                        await db.SaveChangesAsync();
                        dbTran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    error = true;
                }
            }

            if (error)
                return Json("Error");
            else
                return Json("Ok");
        }


        public async Task<ActionResult> ProjectManagement(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var entity = await db.Projects.FindAsync(id);

            if (entity == null)
                return RedirectToAction("Index");

            var ProjectContacts = await db.Contacts.Where(x => x.ProjectId == id).Select(z => new ContactModel
            {
                Company = z.Company,
                Title = z.Title,
                SIC = z.SIC,
                City = z.City,
                State = z.State,
                TimeZone = z.TimeZone,
                InHold = z.InHold
            }).ToListAsync();

            var Project = new ProjectManagementViewModel
            {
                Id = entity.Id,
                Description = entity.Description,
                ProjectSICs = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.SIC) ? "Blank" : x.SIC, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
                ProjectTitles = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.Title) ? "Blank" : x.Title, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
                ProjectCompanies = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.Company) ? "Blank" : x.Company, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
                ProjectCities = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.City) ? "Blank" : x.City, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
                ProjectStates = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.State) ? "Blank" : x.State, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
                ProjectTimeZones = ProjectContacts.Select(x => new ProjectManagementBooleanModel { Property = string.IsNullOrEmpty(x.TimeZone.Name) ? "Blank" : x.TimeZone.Name, TimeZone = x.TimeZone, IsChecked = !x.InHold }).GroupBy(x => x.Property).Select(g => new ProjectManagementBooleanModel { Property = g.Key, TimeZone = g.First().TimeZone, IsChecked = g.Sum(c => c.IsChecked ? 1 : 0) > 0 }).Distinct(new ProductComparer()).ToList(),
            };

            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier", entity.CampaignId);
            return View(Project);
        }

        [HttpPost]
        public async Task<JsonResult> ProjectManagement(ProjectManagementViewModel model)
        {
            if (model == null)
                return Json("Error");
            Boolean error = false;
            Boolean filtered = false;
            Int32 queryCount = 0;
            StringBuilder sb = new StringBuilder();
            var entities = db.Contacts.Where(x => x.ProjectId == model.ProjectId).ToList();
            List<String> queryList = new List<String>();

            try
            {

                //Cities
                if (model.ProjectCities != null)
                    foreach (var item in model.ProjectCities)
                    {
                        if (item.Property == "Blank")
                        {
                            if (sb.Length <= 0)
                                sb.Append($"City = \'\' OR City IS NULL");
                            else
                                sb.Append($" OR City = \'\' OR City IS NULL");
                        }
                        else
                        {
                            if (sb.Length <= 0)
                                sb.Append($"City = @{queryCount++}");
                            else
                                sb.Append($" OR City = @{queryCount++}");
                            queryList.Add(item.Property);
                        }

                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    filtered = true;
                    sb.Clear();
                    queryCount = 0;
                }


                //Titles
                if (model.ProjectTitles != null)
                    foreach (var item in model.ProjectTitles)
                    {

                        if (item.Property == "Blank")
                        {
                            if (sb.Length <= 0)
                                sb.Append($"Title = @{queryCount++} OR Title = NULL");
                            else
                                sb.Append($" OR Title = @{queryCount++} OR Title = NULL");
                            queryList.Add(String.Empty);
                        }
                        else
                        {
                            if (sb.Length <= 0)
                                sb.Append($"Title = @{queryCount++}");
                            else
                                sb.Append($" OR Title = @{queryCount++}");
                            queryList.Add(item.Property);
                        }
                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    filtered = true;
                    sb.Clear();
                    queryCount = 0;
                }

                //SICS
                if (model.ProjectSICs != null)
                    foreach (var item in model.ProjectSICs)
                    {

                        if (item.Property == "Blank")
                        {
                            if (sb.Length <= 0)
                                sb.Append($"SIC = @{queryCount++} OR SIC = NULL");
                            else
                                sb.Append($" OR SIC = @{queryCount++} OR SIC = NULL");
                            queryList.Add(String.Empty);
                        }
                        else
                        {
                            if (sb.Length <= 0)
                                sb.Append($"SIC = @{queryCount++}");
                            else
                                sb.Append($" OR SIC = @{queryCount++}");
                            queryList.Add(item.Property);
                        }
                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    filtered = true;
                    sb.Clear();
                    queryCount = 0;
                }


                //COMPANIES
                if (model.ProjectCompanies != null)
                    foreach (var item in model.ProjectCompanies)
                    {

                        if (item.Property == "Blank")
                        {
                            if (sb.Length <= 0)
                                sb.Append($"Company = @{queryCount++} OR Company = NULL");
                            else
                                sb.Append($" OR Company = @{queryCount++} OR Company = NULL");
                            queryList.Add(String.Empty);
                        }
                        else
                        {
                            if (sb.Length <= 0)
                                sb.Append($"Company = @{queryCount++}");
                            else
                                sb.Append($" OR Company = @{queryCount++}");

                            queryList.Add(item.Property);
                        }
                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    filtered = true;
                    sb.Clear();
                    queryCount = 0;
                }

                //STATES
                if (model.ProjectStates != null)
                    foreach (var item in model.ProjectStates)
                    {
                        if (item.Property == "Blank")
                        {
                            if (sb.Length <= 0)
                                sb.Append($"State = @{queryCount++} OR State = NULL");
                            else
                                sb.Append($" OR State = @{queryCount++} OR State = NULL");
                            queryList.Add(String.Empty);
                        }
                        else
                        {
                            if (sb.Length <= 0)
                                sb.Append($"State = @{queryCount++}");
                            else
                                sb.Append($" OR State = @{queryCount++}");
                            queryList.Add(item.Property);
                        }

                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    queryCount = 0;
                    filtered = true;
                    sb.Clear();
                }


                //TIME ZONES
                if (model.ProjectTimeZonesId != null)
                    foreach (var item in model.ProjectTimeZonesId)
                    {
                        if (sb.Length <= 0)
                            sb.Append($"TimeZoneId = @{queryCount++}");
                        else
                            sb.Append($" OR TimeZoneId = @{queryCount++}");
                        queryList.Add(item.ToString());
                    }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    entities = entities.AsQueryable().Where(sb.ToString(), queryList.ToArray()).ToList();
                    queryList.Clear();
                    queryCount = 0;
                    filtered = true;
                    sb.Clear();
                }

                var everyOne = await db.Contacts.Where(x => !x.IsDeleted && x.ProjectId == model.ProjectId).ToListAsync();
                foreach (var item in everyOne)
                {
                    item.InHold = true;
                    db.Entry(item).State = EntityState.Modified;
                }

                if (filtered)
                    foreach (var item in entities)
                    {
                        item.InHold = false;
                        db.Entry(item).State = EntityState.Modified;
                    }


                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = true;
                AddAlert($"Oops! something went wrong. Error code: {ex.HResult}", "Details", this.GetType().ToString(), AlertType.error, ex);
            }


            if (error)
                return Json("Error");
            else
                return Json("Ok");
        }

    }



    class ProductComparer : IEqualityComparer<ProjectManagementBooleanModel>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ProjectManagementBooleanModel x, ProjectManagementBooleanModel y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Property.ToLower() == y.Property.ToLower();
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ProjectManagementBooleanModel model)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(model, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = model.Property == null ? 0 : model.Property.ToLower().GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName;
        }

    }
}
