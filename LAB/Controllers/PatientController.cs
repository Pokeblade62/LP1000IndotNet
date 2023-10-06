
using LAB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using XapauServer.Helpers;

namespace LAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly DataContext _context;

        public PatientController(DataContext context)
        {
            _context = context;
        }


        //add

        [HttpPost]
        public async Task<ActionResult<CustomResponse<Patient>>> AddPatient(Patient newPatient)
        {

            Console.WriteLine(newPatient);
            string jsonString = JsonConvert.SerializeObject(newPatient, Formatting.Indented);

            Console.WriteLine(jsonString);

            try
            {
                await _context.patient.AddAsync(newPatient);
                await _context.SaveChangesAsync();

                var patient = _context.patient.FirstOrDefault(p => p.Email == newPatient.Email);

                var response = new CustomResponse<Patient>
                {
                    status = true,
                    message = "New Patient Added",
                    data = patient
                };

                return Ok(response);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while saving the entity changes.");
            }
        }



        //update

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomResponse<Patient>>> UpdatePatient(int id, Patient updatedPatient)
        {
            try
            {
                var currentPatient = await _context.patient.FindAsync(id);
                if (currentPatient==null)
                {
                    return NotFound("Patient not found");
                }

                currentPatient.Name = updatedPatient.Name;
                currentPatient.Dateofbirth = updatedPatient.Dateofbirth;
                currentPatient.Email = updatedPatient.Email;
                currentPatient.Allergy = updatedPatient.Allergy;
                currentPatient.Contact = updatedPatient.Contact;


                await _context.SaveChangesAsync();



                var response = new CustomResponse<Patient>
                {
                    status = true,
                    message = "Patient Updated",
                    data = currentPatient
                };

                return Ok(response);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while saving the entity changes.");
            }
        }


        //get

        [HttpGet]
        public IActionResult GetPatients()
        {
            try
            {
                var patients = _context.patient.ToList();
                return Ok(patients);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while fetching patient data.");
            }
        }

        //delete

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomResponse<Patient>>> DeletePatient(int id, Patient updatedPatient)
        {
            try
            {
                var currentPatient = await _context.patient.FindAsync(id);
                if (currentPatient == null)
                {
                    return NotFound("Patient not found");
                }

                _context.patient.Remove(currentPatient);


                await _context.SaveChangesAsync();



                var response = new CustomResponse<Patient>
                {
                    status = true,
                    message = "Patient Deleted"
                };

                return Ok(response);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while saving the entity changes.");
            }
        }



    }
}
