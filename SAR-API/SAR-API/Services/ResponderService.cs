using SAR_API.Domains;
using SAR_API.DTOs;
using SAR_API.Repositories;

namespace SAR_API.IncidentService;

public class ResponderService : IResponderService
{
    private readonly IUserRepository _userRepository;
    private readonly IResponderRepository _responderRepository;
    
    public ResponderService(
        IUserRepository userRepository,
        IResponderRepository responderRepository
        )
    {
        _userRepository = userRepository;
        _responderRepository = responderRepository;
    }
    
    public async Task CreateResponder(AssignUserRequest request)
    {
        // Create full name for responder
        string responderName = request.FirstName + " " + request.LastName;
        
        // Grab User Id using email
        var userId = await _userRepository.FindUserIdByEmail(request.Email);
        
        // Convert Guid to string
        string responderId = Guid.NewGuid().ToString();
        string agencyId = request.AgencyId.ToString();
        string userIdGuid = Guid.Parse(userId).ToString();
        
        Responder responder = new Responder
        {
            ResponderId = responderId,
            ResponderName = responderName,
            BirthDate = request.BirthDate,
            Phone = request.Phone,
            Province = request.Province,
            AgencyId = agencyId,
            CheckedIn = false,
            UserId = userIdGuid
        };
        
        // Add responder to database
        int dbResponse = await _responderRepository.AddResponder(responder);
        
        if (dbResponse == 0)
        {
            throw new Exception("Failed to add responder to database.");
        }
    }
    
    public async Task<string> GetUserIdByEmail(GetUserIdRequest request)
    {
        var userId = await _userRepository.FindUserIdByEmail(request.Email);
        
        if (userId == null)
        {
            throw new Exception("User not found.");
        }
        
        return userId;
    }
    
    public async Task<string> GetResponderIdByEmail(GetUserIdRequest request)
    {
        var responderId = await _responderRepository.FindUserIdByEmail(request.Email);
        
        if (responderId == null)
        {
            throw new Exception("Responder not found.");
        }
        
        return responderId;
    }

    public async Task<List<ResponderDTO>> GetAllResponders()
    {
        // Get responders from the database
        var responders = await _responderRepository.GetAllResponders();
        
        // Check to see if any were found
        if (responders == null)
        {
            throw new Exception("No responders found.");
        }
        
        // Convert to DTO
        List<ResponderDTO> responderDTOs = new List<ResponderDTO>();
        foreach (var responder in responders)
        {
            ResponderDTO dto = new ResponderDTO
            {
                ResponderId = responder.ResponderId,
                CheckedIn = responder.CheckedIn,
                ResponderName = responder.ResponderName,
                Phone = responder.Phone
            };
            
            responderDTOs.Add(dto);
        }
        
        // Return the list of responders
        return responderDTOs;
    }
}