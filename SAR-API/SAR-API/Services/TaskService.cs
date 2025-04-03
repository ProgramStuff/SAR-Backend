using SAR_API.Domains;
using SAR_API.DTOs;
using SAR_API.Repositories;

namespace SAR_API.IncidentService;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public async Task CreateTask(NewTaskRequest request)
    {
        // Add task to database
        string taskId = Guid.NewGuid().ToString();
        
        // Create new task object
        TaskDTO task = new TaskDTO
        {
            TaskId = taskId,
            TaskName = request.TaskName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OperationalPeriod = request.OpId,
            Description = request.Description
        };
        
        // Save task to database
        int result = await _taskRepository.CreateTask(task);
        
        // Check if task was created successfully (result == 0 if failed)
        if (result == 0)
        {
            throw new Exception("Failed to create task");
        }
        
        // Add team members to task
        string teamId = Guid.NewGuid().ToString();
        
        // foreach (var team in request.Team)
        // {
        //     TeamDTO teamDTO = new TeamDTO
        //     {
        //         TeamId = teamId,
        //         ResponderId = team.,
        //         StartDate = team.StartDate,
        //         EndDate = team.EndDate,
        //         TaskId = taskId
        //     };
        //     
        //     // Save team to database
        //     int teamResult = await _taskRepository.CreateTeam(teamDTO);
        //     
        //     // Check if team was created successfully (teamResult == 0 if failed)
        //     if (teamResult == 0)
        //     {
        //         throw new Exception("Failed to create team");
        //     }
        // }
    }
    
    public async Task<ViewTaskDTO> GetTaskView(string taskId)
    {
        // Get task from database
        TaskIncident task = await _taskRepository.GetTaskView(taskId);
        
        // Check if task exists
        if (task == null)
        {
            throw new Exception("Task not found");
        }
        
        // Get team assigned to task
        TeamDetailsDTO team = await _taskRepository.GetTeam(taskId);
        
        // Create view task object
        ViewTaskDTO viewTask = new ViewTaskDTO
        {
            TaskId = task.TaskId,
            TaskName = task.TaskName,
            StartDateTime = task.StartDatetime,
            EndDateTime = task.EndDatetime,
            OpId = task.OperationalPeriodId,
            Description = task.Description,
            Teams = team
        };
        
        return viewTask;
    }
}