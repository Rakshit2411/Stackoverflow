using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Stackoverflow.Models;

namespace Stackoverflow.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.ApplicationUser);
            return View(questions.ToList());
        }

        [AllowAnonymous]
        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title, Details")] Question question)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                question.UserId = User.Identity.GetUserId();
                question.Timeposted = DateTime.Now;
                question.Votes = 0;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,UserId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /**************************************************************************************/
        //Post Answer
        public ActionResult PostAnswer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost]
        public ActionResult PostAnswer(int id, string content)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }

            Answer answer = new Answer
            {
                UserId = User.Identity.GetUserId(),
                QuestionId = id,
                Votes = 0,
                Content = content
            };
            var que = db.Questions.Find(id);
            if (que.UserId != User.Identity.GetUserId())
            {
                db.Answers.Add(answer);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id });
        }

        /*************************************************************************************/
        //Vote count

        [HttpPost]
        public ActionResult VoteIncrease(int? id, int? questionId)
        {
            if (id == null && questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null && questionId != null)
            {
                var question = db.Questions.Find(questionId);
                if (question == null)
                {
                    return HttpNotFound();
                }
                if (question.UserId != User.Identity.GetUserId())
                {
                    question.Votes++;
                    var user = db.Users.Find(question.UserId);
                    user.Reputation += 5;
                    if (ModelState.IsValid)
                    {
                        db.Entry(question).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = questionId });
                    }
                }
                else
                {
                    return RedirectToAction("Details", new { id = questionId });
                }

            }
            else if (id != null && questionId == null)
            {
                var answer = db.Answers.Find(id);
                if (answer == null)
                {
                    return HttpNotFound();
                }
                if (answer.UserId != User.Identity.GetUserId())
                {
                    answer.Votes++;
                    var user = db.Users.Find(answer.UserId);
                    user.Reputation += 5;
                    if (ModelState.IsValid)
                    {
                        db.Entry(answer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = answer.QuestionId });
                    }
                }
                else
                {
                    return RedirectToAction("Details", new { id = answer.QuestionId });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult VoteDecrease(int? id, int? questionId)
        {
            if (id == null && questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null && questionId != null)
            {
                var question = db.Questions.Find(questionId);
                if (question == null)
                {
                    return HttpNotFound();
                }
                if (question.UserId != User.Identity.GetUserId())
                {
                    question.Votes--;
                    var user = db.Users.Find(question.UserId);
                    user.Reputation -= 5;
                    if (ModelState.IsValid)
                    {
                        db.Entry(question).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = questionId });
                    }
                }
                else
                {
                    return RedirectToAction("Details", new { id = questionId });
                }
            }
            else if (id != null && questionId == null)
            {
                var answer = db.Answers.Find(id);
                if (answer == null)
                {
                    return HttpNotFound();
                }
                if (answer.UserId != User.Identity.GetUserId())
                {
                    answer.Votes--;
                    var user = db.Users.Find(answer.UserId);
                    user.Reputation -= 5;
                    if (ModelState.IsValid)
                    {
                        db.Entry(answer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = answer.QuestionId });
                    }
                }
                else
                {
                    return RedirectToAction("Details", new { id = answer.QuestionId });
                }
            }
            return RedirectToAction("Index");
        }

        /*************************************************************************************/
        //post comment

        public ActionResult PostCommentOnQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost]
        public ActionResult PostCommentOnQuestion(int id, string content)
        {
            Comment comment = new Comment
            {
                QuestionId = id,
                Content = content
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
        }

        public ActionResult PostCommentOnAnswer(int? id, int? questionId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.FirstOrDefault(a => a.Id == id && a.QuestionId == questionId);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        [HttpPost]
        public ActionResult PostCommentOnAnswer(int id, string content)
        {
            Comment comment = new Comment
            {
                AnswerId = id,
                Content = content
            };
            var answer = db.Answers.Find(id);
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = answer.QuestionId });
        }

        /*************************************************************************************/
        //Add tag to question.

        public ActionResult AddTagToQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name");
            return View(question);
        }

        [HttpPost]
        public ActionResult AddTagToQuestion(int id, int TagId)
        {
            QuestionTag questionTag = new QuestionTag
            {
                TagId = TagId,
                QuestionId = id
            };
            var que = db.Questions.Find(id);
            if (que.UserId == User.Identity.GetUserId())
            {
                db.QuestionTags.Add(questionTag);
                db.SaveChanges();
            }
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name");
            return RedirectToAction("Details", new { id });
        }

        /*************************************************************************************/
        //Show TagQuestions.

        [AllowAnonymous]
        public ActionResult ShowQuestions(Tag tag)
        {
            var question = new List<Question>();
            foreach (var item in tag.Questions)
            {
                question.Add(item.Question);
            }
            return View(question);
        }

        /*************************************************************************************/
        //Sort Questions.

        [AllowAnonymous]
        public ActionResult Sort(string type)
        {
            var questions = from que in db.Questions select que;

            switch (type)
            {
                case "MostAnswer":
                    questions = questions.OrderByDescending(que => que.Answers.Count());
                    break;
                case "LessAnswer":
                    questions = questions.OrderBy(que => que.Answers.Count());
                    break;
                case "Oldest":
                    questions = questions.OrderByDescending(que => que.Timeposted);
                    break;
                default:
                    questions = questions.OrderBy(que => que.Timeposted);
                    break;
            }
            return View("../Tags/ShowQuestions", questions.ToList());
        }

        /*************************************************************************************/
        //Useful answer.

        [HttpPost]
        public ActionResult UsefulAnswer(int? id, int? questionId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Answer answer = db.Answers.FirstOrDefault(a => a.Id == id && a.QuestionId == questionId);
            if (answer == null)
            {
                return HttpNotFound();
            }
            if (answer.Question.UserId == User.Identity.GetUserId())
            {
                answer.Isaccepted = true;
                if (ModelState.IsValid)
                {
                    db.Entry(answer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = answer.QuestionId });
                }
            }
            else
            {
                return RedirectToAction("Details", new { id = answer.QuestionId });
            }
            return RedirectToAction("Index");
        }
    }
}