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
		[HttpGet]
        public async Task<ActionResult> Edit(int id)
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
			WHERE i.Id = {id}
			";
			using (IDbConnection conn = Connection)
			{
				Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
				return View(instructor);
			}
		}

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Instructor instructor)
        {
            try
            {
				// TODO: Add update logic here
				string sql = $@"
                    UPDATE INSTRUCTOR
                    SET FirstName = '{instructor.FirstName}',
                        LastName = '{instructor.LastName}',
                        SlackHandle = '{instructor.SlackHandle}',
						Specialty = '{instructor.Specialty}'
                    WHERE Id = {id}";

				using (IDbConnection conn = Connection)
				{
					int rowsAffected = await conn.ExecuteAsync(sql);
					if (rowsAffected > 0)
					{
						return RedirectToAction(nameof(Index));
					}
					return BadRequest();

				}
			}
            catch
            {
                return View();
            }
        }

		// GET: Instructors/Delete/5
		[HttpGet]
		public async Task<ActionResult> DeleteConfirm(int id)
        {
			string sql = $@"
			SELECT
				i.Id,
				i.FirstName,
				i.LastName,
				i.SlackHandle,
				i.Specialty
			FROM Instructor i
			WHERE i.Id = {id}
			";
			using (IDbConnection conn = Connection)
			{
				Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
				return View(instructor);
			}
		}

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int id)
		{
			string presql = $@"DELETE FROM StudentExercise WHERE InstructorId = {id}";
			string sql = $@"DELETE FROM Instructor WHERE Id = {id}";

			using (IDbConnection conn = Connection)
			{
				int fkrowsAffected = await conn.ExecuteAsync(presql);
				int rowsAffected = await conn.ExecuteAsync(sql);
				if (rowsAffected > 0)
				{
					return RedirectToAction(nameof(Index));
				}
				throw new Exception("No rows affected");
			}
		}
	}
}