using BusinessLogic.Interfaces;
using DataAccess.Models;

namespace EHR.Helper
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        public AuthService(IUserRepository userRepository,IPatientRepository patientRepository)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
        }

        public async Task<bool> RegisterPatient(Patient model)
        {
            var patient = await _patientRepository.GetByPatientNameAsync(model.Username);
            if ( patient != null)
                return false; 

            await _patientRepository.AddAsync(model);
            int Result = await _patientRepository.CompleteAsync();
            if (Result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateStaff(string Username, string password)
        {
            var Staff = await _userRepository.GetByUserNameAsync(Username);
            if (Staff == null) return false;

            return Staff.Password == password;
        }
        public async Task<bool> ValidatePatient(string Username, string password)
        {
            var Patient = await _patientRepository.GetByPatientNameAsync(Username);
            if (Patient == null) return false;

            return Patient.Password == password;
        }



    }
}
