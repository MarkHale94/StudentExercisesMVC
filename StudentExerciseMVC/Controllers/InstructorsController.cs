using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {
		private readonly IConfiguration _config;

		public InstructorsController(IConfiguration config)
		{
			_config = config;
		}

		public IDbConnection Connection
		{
			get
			{
				return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			}
		}
		// GET: Instructors
		public async Task<ActionResult> Index()
        {
			using (IDbConnection conn = Connection)
			{
				IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
				SELECT i.Id,
				i.FirstName,
				i.LastName,
				i.SlackHandle,
				i.CohortId,
				i.Specialty
				FROM Instructor i
				");
				return View(instructors);
			}
        }

        // GET: Instructors/Details/5
        public async Task<ActionResult> Details(int id)
        {
			string sql = $@"
			SELECT
				i.Id,
				i.FirstName,
				i.LastName,
				i.SlackHandle,
				i.Specialty,
				i.CohortId
				FROM Instructor i
				Where i.Id = {id}
			";
			using (IDbConnection conn = Connection)
			{
				Instructor instructor = await
				conn.QueryFirstAsync<Instructor>(sql);
				return View(instructor);
			}
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}