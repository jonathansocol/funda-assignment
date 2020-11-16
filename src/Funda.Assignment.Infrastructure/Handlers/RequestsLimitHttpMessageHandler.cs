using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Assignment.Infrastructure.Handlers
{
    public class RequestsLimitHttpMessageHandler : DelegatingHandler
    {
        private readonly List<DateTimeOffset> _history = new List<DateTimeOffset>();
        private readonly int _maxRequests;
        private readonly int _timelapse;

        public RequestsLimitHttpMessageHandler(int maxRequests, int timelapse)
        {
            _maxRequests = maxRequests;
            _timelapse = timelapse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            lock (_history)
            {
                _history.Add(DateTime.UtcNow);

                while (_history.Count > _maxRequests)
                {
                    _history.RemoveAt(0);
                }
            }

            await PreventRequestRateLimits();

            return await base.SendAsync(request, cancellationToken);
        }

        public async Task PreventRequestRateLimits()
        {
            if (_history.Count() < _maxRequests)
            {
                return;
            }

            var now = DateTime.UtcNow;
            var dateTimeBeforeTimelapse = now.AddMilliseconds(-_timelapse);

            var recordsDuringTimelapse = _history
                .Where(x => x.DateTime > dateTimeBeforeTimelapse)
                .ToList();

            if (recordsDuringTimelapse.Count() == _maxRequests)
            {
                var ts = recordsDuringTimelapse.Last() - recordsDuringTimelapse.First();

                await Task.Delay((int)ts.TotalMilliseconds);
            }
        }
    }
}
