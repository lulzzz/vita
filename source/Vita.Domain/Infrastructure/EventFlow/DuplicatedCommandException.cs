using System;

namespace Vita.Domain.Infrastructure.EventFlow {
    public class DuplicatedCommandException : Exception {
        public DuplicatedCommandException() {
        }

        public DuplicatedCommandException(string message)
            : base(message) {
        }

        public DuplicatedCommandException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}
