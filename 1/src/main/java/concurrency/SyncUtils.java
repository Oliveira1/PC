package concurrency;

public final class SyncUtils {

	public static int AdjustTimeout(long lastTime, int timeout) {
		
		if (timeout == Integer.MAX_VALUE) return timeout;
        long now = System.currentTimeMillis();
        long elapsed = (now == lastTime) ? 1 : now - lastTime;
        if (elapsed >= timeout)
        {
            timeout = 0;
        }
        else
        {
            timeout -= elapsed;
        }
        return timeout;
	}
	

}
