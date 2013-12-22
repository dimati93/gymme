using System;

using Gymme.Data.Models;

namespace Gymme.Resources
{
    public class Intelligent
    {
        private static readonly TimeSpan TrainingExpTime = TimeSpan.FromHours(12);

        public static bool IsTrainingExperate(Training training)
        {
            if (training.Status == TrainingStatus.Started && (DateTime.Now - training.StartTime) > TrainingExpTime)
            {
                return true;
            }

            return false;
        }
    }
}