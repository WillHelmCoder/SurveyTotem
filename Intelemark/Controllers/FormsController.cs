using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Services;
using Intelemark.Utilities;
using Microsoft.Ajax.Utilities;

namespace Intelemark.Controllers
{
    [Authorize]
    public class FormsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            try
            {

                var entities = await db.Forms
                    .Where(x => !x.IsDeleted).Select(x => new DTO.Forms.Output
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        CampaignIdentifier = x.Campaign.Identifier,
                        CampaignDescription = x.Campaign.Description,
                    }).ToListAsync();


                return View(entities);
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
        public JsonResult UploadImage()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                string fileName = file.FileName;
                string[] fileParts = fileName.Split('/');
                string path = "~/Content/QuestionImages/" + fileParts[0];
                bool exists = System.IO.Directory.Exists(Server.MapPath(path));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(path));
                file.SaveAs(Server.MapPath("~/Content/QuestionImages/") + fileName);
            }
            return Json("Uploaded " + Request.Files.Count + " files");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Form
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CampaignId = model.CampaignId,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    db.Forms.Add(entity);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create", this.GetType().ToString(), AlertType.error, e);
            }

            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return View(model);
        }

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var entity = await db.Forms.FindAsync(id);
                if (entity == null)
                    return HttpNotFound();

                var model = new FormModel
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    CampaignId = entity.CampaignId,
                    Campaign = entity.Campaign,
                    QuestionList = entity.Questions.Select(y => new QuestionModel
                    {
                        Question = y.Name,
                        TypeId = y.TypeId,
                        Order = y.Order,
                        Points = y.Points,
                        Image = y.Image,
                        AnswerList = y.Answers.Select(z => new AnswerModel
                        {
                            Answer = z.Name,
                            Order = z.Order
                        }).ToList()
                    }).ToList()
                };

                ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier", model.CampaignId);
                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Details", this.GetType().ToString(), AlertType.error, e);
                return View(new FormModel());
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var res = await Service<Form>.Delete(new Form { Id = id });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> CreateForm(FormModel model, HttpPostedFileBase imageFile)
        {
            //var path = Path.Combine(Server.MapPath("~/Content/QuestionImages/"), System.IO.Path.GetFileName(imageFile.FileName));
            //imageFile.SaveAs(path);
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Form
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CampaignId = model.CampaignId,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    db.Forms.Add(entity);
                    await db.SaveChangesAsync();
                    var Questions = new List<Question>();

                    int i = 1;
                    foreach (var item in model.Questions)
                    {
                        Questions.Add(new Question
                        {
                            FormId = entity.Id,
                            Name = item.Question,
                            TypeId = item.TypeId,
                            Order = i++,
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            IsDeleted = false,
                            Points = item.Points,
                            Image = item.Image
                        });
                    }

                    db.Questions.AddRange(Questions);
                    await db.SaveChangesAsync();

                    i = 1;
                    var Answers = new List<Answer>();
                    int tmp = 0;
                    foreach (var question in model.Questions)//model - answers
                    {
                        foreach (var item in question.Answers)//
                        {
                            if (!string.IsNullOrEmpty(item.Answer) && item.Answer != "----AnswerHidden----")
                            {
                                Answers.Add(new Answer
                                {
                                    QuestionId = Questions[tmp].Id,//questio.id
                                    Name = item.Answer,
                                    Order = i++,
                                    CreationDate = DateTime.Now,
                                    LastUpdate = DateTime.Now,
                                    IsDeleted = false,
                                });
                            }
                        }
                        i = 1;
                        tmp++;
                    }

                    db.Answers.AddRange(Answers);
                    await db.SaveChangesAsync();
                    return Json("Succesfuly saved!");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "CreateForm", this.GetType().ToString(), AlertType.error, e);
                return Json(e);
            }

            var errors = new List<String>();
            foreach (var item in ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(errors);
        }


        #region "SelectableAnswer"

        public ActionResult AddSelectableAnswer()
        {
            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            ViewBag.FormId = new SelectList(new List<FormModel>(), "Id", "Name");
            ViewBag.QuestionId = new SelectList(new List<QuestionModel>(), "Id", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult AddSelectableAnswer(SelectableAnswerViewModel model)
        {

            if (ModelState.IsValid)
            {
                var entity = new Answer
                {
                    Name = model.NewAnswer,
                    QuestionId = model.QuestionId,
                    Order = 30,
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now,

                };

                db.Answers.Add(entity);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            ViewBag.FormId = new SelectList(new List<FormModel>(), "Id", "Name");
            ViewBag.QuestionId = new SelectList(new List<QuestionModel>(), "Id", "Name");
            return View();
        }


        [HttpGet]
        public JsonResult GetForms(Int32 CampaignId)
        {
            var userForms = new List<Form>();
            userForms = db.Forms.Where(x => !x.IsDeleted && x.CampaignId == CampaignId).ToList();
            var Forms = userForms.Select(x => new { x.Id, x.Name });
            return Json(Forms, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuestions(Int32 FormId)
        {
            var questions = new List<Question>();
            questions = db.Questions.Where(x => !x.IsDeleted && x.TypeId == Utilities.QuestionTypes.SelectableAnswer).ToList();
            var questionsAvailable = questions.Select(x => new { x.Id, x.Name });
            return Json(questionsAvailable, JsonRequestBehavior.AllowGet);
        }




        #endregion

    }
}
