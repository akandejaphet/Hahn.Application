using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FluentValidation;

namespace Hahn.ApplicatonProcess.May2020.Domain.Models
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator()
        {
            RuleFor(applicant => applicant.Name).NotNull().MinimumLength(5);
            RuleFor(applicant => applicant.FamilyName).NotNull().MinimumLength(5);
            RuleFor(applicant => applicant.Address).NotNull().MinimumLength(10);
            //RuleFor(applicant => applicant.CountryOfOrigin).NotNull().MinimumLength(5);
            RuleFor(applicant => applicant.EMailAdress).NotNull().EmailAddress();
            RuleFor(applicant => applicant.Age).ExclusiveBetween(20,60);
            RuleFor(applicant => applicant.Hired).NotNull();
            RuleFor(a => a.CountryOfOrigin).NotNull().Must((rootObject, list, context) =>
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://restcountries.eu/rest/v2/name/"+list);
                HttpResponseMessage response = client.GetAsync("?fullText=true").Result;
                
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    return true;
                }
                else
                {
                    return false;
                }
            }).WithMessage("must be a valid country"); ;
        }
    }
}
