using BusinessObject;
using Repositories.Interfaces;

namespace SFBMS_API.Services
{
    public class BookingStatusService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<BookingStatusService> _logger;
        private Timer _timer = null!;

        public BookingStatusService(ILogger<BookingStatusService> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }
        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckBookingStatusAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            _logger.LogInformation("Timer for booking status check started.");
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            _logger.LogInformation("Timer for booking status check stopped.");
            return Task.CompletedTask;
        }

        private async void CheckBookingStatusAsync(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    _logger.LogInformation("Checking booking status.");
                    var bookingDetailRepository = scope.ServiceProvider.GetRequiredService<IBookingDetailRepository>();
                    List<BookingDetail> details = (await bookingDetailRepository.GetPendingBookingDetailsForDate(DateTime.Now)).ToList();
                    foreach (var detail in details)
                    {
                        try
                        {
                            if (DateTime.Now >= detail.StartTime)
                            {
                                if (detail.Status == (int)BookingDetailStatus.NotYet)
                                {
                                    detail.Status = (int)BookingDetailStatus.Open;
                                }
                            }

                            if (DateTime.Now >= detail.EndTime)
                            {
                                if (detail.Status == (int)BookingDetailStatus.Open)
                                {
                                    detail.Status = (int)BookingDetailStatus.Absent;
                                }
                            }
                        }
                        catch
                        {
                            _logger.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await bookingDetailRepository.UpdateRange(details.ToArray());
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Booking status check operation was not successful.");
                    _logger.LogError(e, string.Empty, Array.Empty<int>());
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _logger.LogInformation("Disposed timer for booking status check.");
        }
    }
}
