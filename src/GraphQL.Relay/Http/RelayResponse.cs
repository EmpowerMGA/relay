using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.SystemTextJson;

namespace GraphQL.Relay.Http
{

  public class RelayResponse
  {
    public IDocumentWriter Writer { get; set; }
    public bool IsBatched { get; set; }
    public IEnumerable<ExecutionResult> Results { get; set; }

    public IEnumerable<ExecutionError> Errors =>
        Results?.SelectMany(r => r.Errors) ?? new List<ExecutionError>();

    public bool HasErrors => Results.Any(r => r.Errors?.Count > 0);

    public Task WriteAsync(Stream stream, CancellationToken stoppingToken = default)
    {
      return Writer.WriteAsync(
          stream,
          IsBatched ?
          (object)Results :
          Results.FirstOrDefault(),
      stoppingToken);
    }
  }
}
