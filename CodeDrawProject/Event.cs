namespace CodeDrawProject
{
	internal class Event<TArgs>
	{
		public Event(CodeDraw sender)
		{
			this.sender = sender;
		}

		private CodeDraw sender;
		private event EventHandler<TArgs>? @event;

		public void Invoke(object? sender, TArgs args)
		{
			@event?.Invoke(this.sender, args);
		}

		public static Event<TArgs> operator +(Event<TArgs> @event, EventHandler<TArgs> eventHandler)
		{
			@event.@event += eventHandler;
			return @event;
		}

		public static Event<TArgs> operator -(Event<TArgs> @event, EventHandler<TArgs> eventHandler)
		{
			@event.@event -= eventHandler;
			return @event;
		}
	}
}
