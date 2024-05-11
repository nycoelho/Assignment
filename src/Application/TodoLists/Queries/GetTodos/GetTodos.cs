using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;
using Assignment.Domain.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<IList<TodoListDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IList<TodoListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<IList<TodoListDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        // Ger or create cached Todo Lists
        var cachedResult = await _memoryCache.GetOrCreateAsync<IList<TodoListDto>>(Caches.TodosKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _context.TodoLists
                .AsNoTracking()
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
        });
        if (cachedResult is not null)
            return cachedResult;

        // In case the cachedResult is null, revert to default behavior
        return await _context.TodoLists
                .AsNoTracking()
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
    }
}
