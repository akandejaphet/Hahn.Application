using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using FluentValidation.Results;
using System.Net.Http;
using System.Text.Json;
using Hahn.ApplicatonProcess.May2020.Data;

namespace Hahn.ApplicatonProcess.May2020.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly ApplicantContext _applicantContext;

        public ApplicantController(ApplicantContext applicantContext)
        {
            _applicantContext = applicantContext;
        }
        // GET: api/Applicant
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var result = _applicantContext.Applicants.ToList();
            return Ok(result);
        }

        // GET: api/Applicant/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var result = _applicantContext.Applicants.Find(id);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        // POST: api/Applicant
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] Applicant a )
        {
            ApplicantValidator v = new ApplicantValidator();
            ValidationResult result = v.Validate(a);
            if (result.IsValid)
            {
                var qu = _applicantContext.Applicants.Add(a);
                var user = _applicantContext.SaveChanges();
                return Created("/"+a.ID, user);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/Applicant/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Applicant a)
        {
            ApplicantValidator v = new ApplicantValidator();
            ValidationResult result = v.Validate(a);
            if (result.IsValid)
            {
                var queryApplicant = _applicantContext.Applicants.Find(id);
                if (queryApplicant != null)
                {
                    queryApplicant.Address = a.Address;
                    queryApplicant.Age = a.Age;
                    queryApplicant.CountryOfOrigin = a.CountryOfOrigin;
                    queryApplicant.EMailAdress = a.EMailAdress;
                    queryApplicant.FamilyName = a.FamilyName;
                    queryApplicant.Hired = a.Hired;
                    queryApplicant.Name = a.Name;
                    _applicantContext.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var queryApplicant = _applicantContext.Applicants.Remove(new Applicant { ID = id});
            _applicantContext.SaveChanges();
        }
    }
}
