using Bakalarka;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PramSimulator_WebApp.Components.Services
{
    public class PramSimulatorService
    {
        public bool AutoRunning { get; set; }
        public int CurrentLine { get; set; }
        public bool ShouldStartParallel { get; set; } = false;
        public HashSet<int> Breakpoints { get; set; } = new();
        public event Action? OnChanged;
        public event Action? NavigationToParallel;
        public event Action? NavigationToHome;
        public bool NavToParallel { get; set; }
        public bool NavToHome { get; set; }
        public Timer timer { get; set; }

        public void AutoRun(PRAM pram)
        {
            if (AutoRunning)
                return;

            AutoRunning = true;
            timer = new Timer(NextStep, pram, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            OnChanged?.Invoke();
        }
        public void StopAutoRun()
        {
            if (!AutoRunning) return;

            timer?.Change(Timeout.Infinite, Timeout.Infinite); // Zastaví timer
            timer?.Dispose();
            timer = null;
            AutoRunning = false;
            OnChanged?.Invoke(); // Notifikace UI
        }
        public void NextStep(object? state)
        {
            if (!AutoRunning) return;
            var pram = (PRAM)state;
            var CurrentLine = pram.CurrentCodeLine;
            if(!pram.Breakpoints.Contains(CurrentLine))
            {
                if(pram.ParallelExecution)
                {
                    pram.ExecuteNextParallelStep();
                    if(!pram.ParallelExecution)
                    {
                        NavToHome = true;
                    }
                    if (!pram.ParallelExecution)
                        pram.CurrentCodeLine++;
                    foreach (var p in pram.ActiveProcessors)
                    {
                        if (p.Running)
                        {
                            pram.CurrentCodeLine = pram.ParallelExecution ? p.Program.instructions[p.InstructionPointer].CodeLineIndex : pram.MainProgram.instructions[pram.InstructionPointer].CodeLineIndex;
                            break;
                        }
                    }


                }
                else
                {
                    pram.ExecuteNextInstruction();
                    if (pram.InstructionPointer >= 0)
                        pram.CurrentCodeLine = pram.ParallelExecution ? pram.Processors[0].Program.instructions[pram.Processors[0].InstructionPointer].CodeLineIndex : pram.MainProgram.instructions[pram.InstructionPointer].CodeLineIndex;
                    if(pram.ParallelExecution)
                    {
                        NavToParallel = true;
                    }
                }
                OnChanged?.Invoke();
                if (NavToHome)
                {
                    NavToHome = false;
                    NavigationToHome?.Invoke();
                }
                if(NavToParallel)
                {
                    NavToParallel = false;
                    NavigationToParallel?.Invoke();
                }
            }
            else
            {
                StopAutoRun();
            }
        }
    }
}
