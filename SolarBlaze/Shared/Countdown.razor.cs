using Microsoft.AspNetCore.Components;
using System.Timers;

namespace SolarBlaze.Shared
{
    public partial class Countdown
        : ComponentBase, IDisposable
    {
        private System.Timers.Timer _timer = null!;
        private int _secondsRemaining;

        protected string Time { get; set; } = "00:00";

        [Parameter]
        public EventCallback TimerOut { get; set; }

        [Parameter]
        public int DurationSeconds { get; set; }

        protected override void OnInitialized()
        {
            _secondsRemaining = DurationSeconds;
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
                _secondsRemaining = DurationSeconds;
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
    }
}
