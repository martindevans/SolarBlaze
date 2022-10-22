using Microsoft.AspNetCore.Components;
using System.Timers;

namespace SolarBlaze.Shared
{
    public partial class Countdown
        : ComponentBase, IDisposable
    {
        private System.Timers.Timer _timer = null!;
        private TimeSpan _duration;
        private int _secondsRemaining;
        
        protected string Time { get; set; } = "00:00";

        [Parameter]
        public EventCallback TimerOut { get; set; }

        protected override void OnInitialized()
        {
            _secondsRemaining = Math.Max(1, (int)_duration.TotalSeconds);
            SetTime();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private async void OnTick(object? sender, ElapsedEventArgs e)
        {
            _secondsRemaining--;

            await InvokeAsync(() =>
            {
                SetTime();
                StateHasChanged();
            });

            if (_secondsRemaining <= 0)
            {
                _secondsRemaining = (int)_duration.TotalSeconds;
                await TimerOut.InvokeAsync();
            }
        }

        private void SetTime()
        {
            Time = TimeSpan.FromSeconds(_secondsRemaining).ToString(@"mm\:ss");
        }

        public void Dispose()
        {
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SetDuration(TimeSpan timespan)
        {
            _duration = timespan;
            _secondsRemaining = (int)_duration.TotalSeconds;
            StateHasChanged();
        }
    }
}
