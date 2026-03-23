using System;

public static class GraveyardEvents
{
    // global events for haunt system

    // called when ghost enters haunt
    public static Action OnHauntStart;

    // called when ghost exits haunt
    public static Action OnHauntEnd;

    // using a static class so everything can access it easy
    // prob not best for huge projects but fine for this
}