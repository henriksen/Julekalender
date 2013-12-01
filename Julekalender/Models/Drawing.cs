using System;

namespace Julekalender.Models
{
    public class Drawing
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public Participant Winner { get; set; }
    }
}