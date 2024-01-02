﻿namespace Core.Application.Exceptions
{
    [Serializable]
    public class ProcessingException : Exception
    {
        public ProcessingException() { }
        public ProcessingException(string message) : base(message) { }
        public ProcessingException(string message, Exception inner) : base(message, inner) { }
    }
}