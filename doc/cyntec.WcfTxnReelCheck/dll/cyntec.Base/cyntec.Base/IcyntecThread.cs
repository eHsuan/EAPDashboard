using System.Threading;

namespace cyntec.Base;

public interface IcyntecThread
{
	int SleepTime { get; set; }

	bool IsInProcess { get; set; }

	string Name { get; set; }

	ThreadPriority Priority { get; set; }

	event ThreadProcessEvent onProcess;

	event ThreadStopEvent onStop;

	event ThreadErrorEvent onError;

	void Start();

	void Stop();
}
