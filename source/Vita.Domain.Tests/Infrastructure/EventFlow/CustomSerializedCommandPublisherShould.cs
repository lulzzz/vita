using System;
using System.Collections.Generic;
using System.Text;
using EventFlow;
using EventFlow.Core;
using EventFlow.Logs;
using FluentAssertions;
using Vita.Domain.Charges.Commands;
using Vita.Domain.Infrastructure.EventFlow;
using Xunit;

namespace Vita.Domain.Tests.Infrastructure.EventFlow
{
    public class CustomSerializedCommandPublisherShould
    {
        private CustomSerializedCommandPublisher _serializedCommandPublisher;
        private readonly ILog _log = NSubstitute.Substitute.For<ILog>();
        private readonly IJsonSerializer _jsonSerializer = NSubstitute.Substitute.For<IJsonSerializer>();
        private readonly ICommandBus _commandBus = NSubstitute.Substitute.For<ICommandBus>();

        public CustomSerializedCommandPublisherShould()
        {
            _serializedCommandPublisher = new CustomSerializedCommandPublisher(_log,_jsonSerializer, _commandBus);
        }

        [Fact]
        public void Definitions_contain_commands()
        {
            CustomSerializedCommandPublisher.Definitions.Length.Should().NotBe(0);
            CustomSerializedCommandPublisher.Definitions.Should().Contain(x => x.CommandType == typeof(ImportChargesCommand));
        }
    }
}
