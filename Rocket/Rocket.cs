using System;
using System.Linq;
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
    protected double? maxHeight = null;
    public double MaxHeight {
        get {
            if (maxHeight is null)
                maxHeight = GetMaxHeight();
            return maxHeight.Value;
        }
    }
    public double RocketMass_Kg { get; }
    public double FuelMass_kg { get; protected set; }
    public double Mass => RocketMass_Kg + FuelMass_kg;

    public Rocket((double[] t, double[] MassFlow) fuelExpense, double initialHeight = 0)
    {
        this.FuelExpense = fuelExpense;
        this.InitialHeight = initialHeight;
        this.RocketMass_Kg = EmptyRocketMass_Kg;
        this.FuelMass_kg = TotalFuelMass_Kg;
    }

    public double GetMaxHeight()
    {
        double maxHeight = InitialHeight;
        double maxTime = MaxTime;
        double speed = 0;
        
        double height = InitialHeight;
        for (int i = 0; i <= maxTime; i++)
        {
            double acceleration = Acceleration(i, height, speed);
            
        }

        return maxHeight;
    }

    public double Acceleration(int time, double height, double speed)
        => (EngineThrustForce(time) + DragForce(height, speed) + WeightForce(height)) / Mass;

    public double EngineThrustForce(int time)
        => FuelMassFlow(time) * EngineExhaustSpeed_ms;

    public double FuelMassFlow(int Time)
        => Interp1D.Linear(FuelExpense.time, FuelExpense.MassFlow, Time);

    public double WeightForce(double height)
        => -Mass * Gravity.GetGravity(height);

    public double DragForce(double height, double speed)
        => -(1/2) * Atmosphere.Density(height) * RocketCrossSectionalArea_m2 * Math.Pow(speed, 2) * (speed < 0 ? -1 : 1);
}