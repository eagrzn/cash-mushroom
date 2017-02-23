using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    public class AggregateTests<A> where A : Aggregate, new()
    {
        private A sut;

        [SetUp]
        public void AggregateTestsSetup()
        {
            sut = new A();
        }

        protected void Test(IEnumerable given, Func<A, object> when, Action<object> then)
        {
            then(when(ApplyEvents(sut, given)));
        }

        protected IEnumerable Given(params object[] events)
        {
            return events;
        }

        protected Func<A, object> When<C>(C command) where C : ICommand
        {
            return agg =>
            {
                try
                {
                    return DispatchCommand(command).Cast<object>().ToArray();
                }
                catch (Exception e)
                {
                    return e;
                }
            };
        }

        protected Action<object> Then(params object[] expectedEvents)
        {
            return got =>
            {
                var gotEvents = got as object[];
                if (gotEvents != null)
                {
                    if (gotEvents.Length == expectedEvents.Length)
                        for (var i = 0; i < gotEvents.Length; i++)
                            if (gotEvents[i].GetType() == expectedEvents[i].GetType())
                                Assert.AreEqual(Serialize(expectedEvents[i]), Serialize(gotEvents[i]));
                            else
                            {
                                var e = expectedEvents[i].GetType().Name;
                                var g = gotEvents[i].GetType().Name;
                                var m = $"Incorrect event in results; expected a {e} but got a {g}";
                                Assert.Fail(m);
                            }
                    else if (gotEvents.Length < expectedEvents.Length)
                    {
                        var events = String.Join(", ", EventDiff(expectedEvents, gotEvents));
                        var m = $"Expected event(s) missing: {events}";
                        Assert.Fail(m);
                    }
                    else
                    {
                        var events = String.Join(", ", EventDiff(gotEvents, expectedEvents));
                        var m = $"Unexpected event(s) emitted: {events}";
                        Assert.Fail(m);
                    }
                }
                else if (got is CommandHandlerNotDefiendException)
                    Assert.Fail((got as Exception).Message);
                else
                {
                    var m = $"Expected events, but got exception {got.GetType().Name}";
                    Assert.Fail(m);
                }
            };
        }

        private String[] EventDiff(object[] a, object[] b)
        {
            var diff = a.Select(e => e.GetType().Name).ToList();
            foreach (var remove in b.Select(e => e.GetType().Name))
                diff.Remove(remove);
            return diff.ToArray();
        }

        protected Action<object> ThenFailWith<TException>()
        {
            return got =>
            {
                if (got is TException)
                    Assert.Pass("Got correct exception type");
                else if (got is CommandHandlerNotDefiendException)
                    Assert.Fail((got as Exception).Message);
                else if (got is Exception)
                {
                    var m = $"Expected exception {typeof(TException).Name}, but got exception {got.GetType().Name}";
                    Assert.Fail(m);
                }
                else
                {
                    var m = $"Expected exception {typeof(TException).Name}, but got event result";
                    Assert.Fail(m);
                }
            };
        }

        private IEnumerable DispatchCommand<C>(C c) where C : ICommand
        {
            var handler = sut as IHandleCommand<C>;
            if (handler == null)
            {
                var m = $"Aggregate {sut.GetType().Name} does not yet handle command {c.GetType().Name}";
                throw new CommandHandlerNotDefiendException(m);
            }
            return handler.Handle(c);
        }

        private A ApplyEvents(A agg, IEnumerable events)
        {
            agg.ApplyEvents(events);
            return agg;
        }

        private String Serialize(object obj)
        {
            var ser = new XmlSerializer(obj.GetType());
            var ms = new MemoryStream();
            ser.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            return new StreamReader(ms).ReadToEnd();
        }

        private class CommandHandlerNotDefiendException : Exception
        {
            public CommandHandlerNotDefiendException(String msg) : base(msg) { }
        }
    }
}