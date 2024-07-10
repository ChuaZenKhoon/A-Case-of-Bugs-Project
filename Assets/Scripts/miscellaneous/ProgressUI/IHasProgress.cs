using System;

/**
 * A set of functionality that any class that has some form of progress tracking should have.
 */
public interface IHasProgress {

    public event EventHandler<float> OnActionProgress;

}
