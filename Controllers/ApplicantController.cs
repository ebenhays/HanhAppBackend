using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.May2020.Domain.BusinessLayer.Interfaces;
using Hahn.ApplicatonProcess.May2020.Domain.BusinessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicatonProcess.May2020.Web.Controllers
{
	[EnableCors("MyPolicy")]
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicantController : ControllerBase
	{
		private readonly IApplicant _repository;
		public ApplicantController(IApplicant applicant )
		{
			_repository = applicant;
		}

		/// <summary>
		/// This method creates a new Applicant.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// POST /applicant
		/// {
		///		"name": "Ebenezer Casely Hayford",
		///		"age": 31,
		///		"emailAddress":"shoak1989@yahoo.com",
		///		"countryOfOrigin":"Ghana",
		///		"familyName":"Casely Hayford",
		///		"hired":true,
		///		"address":"P.O.Box, 611, Accra Ghana"
		/// }
		/// </remarks>
		/// <param name="Newapplicant"></param>
		/// <returns>A Newly Created Applicant Type</returns>
		/// <response code="201">Returns the newly created applicant</response>
		/// <response code="400">If there is a validation issue</response> 

		[HttpPost]
		public IActionResult PostApplicant(Applicant Newapplicant)
		{
			var isCreated = _repository.CreateApplicant(Newapplicant);
			if(isCreated)return CreatedAtAction("GetApplicant", new { id = Newapplicant.ID }, Newapplicant);
			return BadRequest();
			
		}

		/// <summary>
		/// This method get an applicant by id
		/// </summary>
		/// <remarks> GET /applicant/{id} </remarks>
		/// <param name="id"></param>
		/// <returns>A Single Applicant</returns>
		/// <response code="200">Returns if applicant is found</response>
		/// <response code="404">If the applicant is not found</response> 
		
		[HttpGet("{id}")]
		public ActionResult<Applicant> GetApplicant(int id)
		{
			var result = _repository.GetApplicant(id);
			if(result == null)
			{
				return NotFound(string.Format("Applicant with ID {0} not found",id));
			}
			return Ok(result);
		}

		/// <summary>
		/// This method deletes an Applicant
		/// </summary>
		/// <remarks> DELETE /applicant/{id} </remarks>
		/// <param name="id"></param>
		/// <returns>Message:Record Successfully Deleted </returns>
		/// <response code="200">if delete was successful</response>
		/// <response code="400">Bad Request</response>
		
		[HttpDelete("{id}")]
		public IActionResult DeleteApplicant(int id)
		{
			var isDeleted = _repository.DeleteApplicant(id);
			if(isDeleted)return Ok("Record Successfully Deleted");
			return BadRequest();
			
		}

		/// <summary>
		/// This method updates an applicant record
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// PUT /applicant/{id}
		/// {
		///		"name": "Ebenezer Casely Hayford",
		///		"age": 31,
		///		"emailAddress":"shoak1989@yahoo.com",
		///		"countryOfOrigin":"Ghana",
		///		"familyName":"Casely Hayford",
		///		"hired":true,
		///		"address":"P.O.Box, 611, Accra Ghana"
		/// }
		/// </remarks>
		/// <param name="applicant"></param>
		/// <param name="id"></param>
		/// <returns>Message:Record Successfully Updated</returns>
		/// <response code="200">if update was successful</response>
		/// <response code="400">Bad Request</response>

		[HttpPut("{id}")]
		public IActionResult PutApplicant(Applicant applicant, int id)
		{
		
			var isUpdated = _repository.UpdateApplicant(applicant, id);
			if(isUpdated)return Ok("Record Successfully Updated");
			return BadRequest();
			
		}
	}
}
