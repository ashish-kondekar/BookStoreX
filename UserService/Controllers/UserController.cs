namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
{
    private static readonly List<UserDto> _users =
    [
        new (1, "Alice", "Smith", "alice@example.com", "Admin"),
        new (2, "Bob", "Johnson", "bob@example.com", "User"),
        new (3, "Carol", "Williams", "carol@example.com", "Manager")
    ];

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <remarks>
    /// Returns a list of all users in the system.
    /// </remarks>
    /// <returns>List of <see cref="UserDto"/></returns>
    /// <response code="200">Returns the list of users</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        await Task.Delay(1000);
        return Ok(_users);
    }

    /// <summary>
    /// Retrieves a user by their unique ID.
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns>A single <see cref="UserDto"/> if found</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">If the user is not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = _users.FirstOrDefault(x => x.Id == id);

        if (user is null)
        {
            return NotFound(new ErrorResponse("User not found", HttpStatusCode.NotFound));
        }

        await Task.Delay(1000);
        return Ok(user);
    }

    /// <summary>
    /// Create a new catalog.
    /// </summary>
    /// <param name="UserDto">User to create.</param>
    /// <returns>The created <see cref="UserDto"/>.</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="400">If any validation fail</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> CreateUser([FromBody] UserRequestDto userDto)
    {
        var newUser = new UserDto(_users.Count + 1, userDto.FirstName, userDto.LastName, userDto.Email, userDto.Role);

        _users.Add(newUser);

        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }

}
