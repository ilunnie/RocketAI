using System;
using System.Linq;
using System.Xml;
using AIContinuous.Nuenv;

namespace AIContinuous.Rocket;

public class Rocket
{
    public static double EmptyRocketMass_Kg { get; } = 750;
    public static double TotalFuelMass_Kg { get; } = 3500;
    public static float RocketDiameter_m { get; } = .6f;
    public static float RocketCrossSectionalArea_m2 => MathF.PI * MathF.Pow(RocketDiameter_m / 2, 2);
    public static float StandardDragCoefficient_m { get; } = .8f;
    public static float EngineExhaustSpeed_ms { get; } = 1916;

    public (double[] time, double[] MassFlow) FuelExpense { get; }
    public double MaxTime => FuelExpense.time.Max();

    public double InitialHeight { get; }
    public double RocketMass_Kg { get; }
    public double FuelMass_kg { get; protected set; }
    public double Mass  => RocketMass_Kg + FuelMass_kg;
    public double Time { get; set; } = 0;
    public double Speed { get; protected set; }
    public double Height { get; protected set; }

    public Rocket((double[] t, double[] MassFlow) fuelExpense, double initialHeight = 0)
    {
        this.FuelExpense = fuelExpense;
        this.InitialHeight = initialHeight;
        this.RocketMass_Kg = EmptyRocketMass_Kg;
        this.FuelMass_kg = Integrate.Romberg(fuelExpense);
    }

    public double Launch(double time, double dt = 1e-1)
    {
        for (double t = 0; t < time; t += dt)
            Next(dt);

        return Height;
    }

    public double LaunchUntilMax(double dt = 1e-1)
    {
        do Next(dt);
        while (Speed > .0);

        return Height;
    }

    public double LaunchUntilGround(double dt = 1e-1)
    {
        double maxHeight = 0;

        do
        {
            Next(dt);
            if (Height > maxHeight)
                maxHeight = Height;
        }
        while (Height > .0);

        return maxHeight;
    }

    public void Next(double dt)
    {
        UpdateSpeed(dt);
        UpdateHeight(dt);
        UpdateMass(dt);

        Time += dt;
    }

    public void UpdateSpeed(double dt)
        => Speed += Acceleration(Time) * dt;

    public void UpdateHeight(double dt)
        => Height += Speed * dt;

    public void UpdateMass(double dt)
        => FuelMass_kg -= .5 * dt * (FuelMassFlow(Time) * FuelMassFlow(Time + dt));

    public double Acceleration(double time)
        => (EngineThrustForce(time) + DragForce(Height, Speed) + WeightForce(Height)) / Mass;

    public double EngineThrustForce(double time)
        => time > FuelExpense.time[^1] ? .0 : FuelMassFlow(time) * EngineExhaustSpeed_ms;

    public double FuelMassFlow(double time)
        => time > FuelExpense.time[^1] ? RocketMass_Kg : Interp1D.Linear(FuelExpense.time, FuelExpense.MassFlow, Time);

    public double WeightForce(double height)
        => -Mass * Gravity.GetGravity(height);

    public static double DragForce(double height, double speed)
    {
        double temp = Atmosphere.Temperature(height);
        double cd = Drag.Coefficient(speed, temp, StandardDragCoefficient_m);
        return -.5 * cd * Atmosphere.Density(height) * RocketCrossSectionalArea_m2 * Math.Pow(speed, 2) * Math.Sign(speed);
    }
}