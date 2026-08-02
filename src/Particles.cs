using Raylib_cs;
using System.Numerics;

namespace ParticleSystem
{
    // 2D PARTICLE SYSTEM
    class PARTICLE2D
    {
        // Types of particle shapes you can use
        public enum Shapes
        {
            Triangle,
            Square,
            Pentagon,
            Hexagon,
            Septagon,
            Circle,
        }

        // Dictionary for mapping shapes to draw methods
        private static readonly Dictionary<Shapes, int> ShapeSides = new()
    {
        {Shapes.Triangle, 3},
        {Shapes.Square, 4},
        {Shapes.Pentagon, 5},
        {Shapes.Hexagon, 6},
        {Shapes.Septagon, 7},
        {Shapes.Circle, 10}
    };

        // Struct of Array system for particles
        protected struct Particles(int size)
        {
            public Vector2[] Position = new Vector2[size];
            public Vector2[] Velocity = new Vector2[size];
            public float[] Size = new float[size];
            public int[] Rotation = new int[size];
            public float[] Age = new float[size];
            public bool[] Alive = new bool[size];
            public byte[] Alpha = new byte[size];
        }

        protected static Particles particles; // the SoA of particles
        public static Shapes CurrentShape;

        public PARTICLE2D(int numberOfParticles, Shapes shape)
        {
            particles = new(numberOfParticles);
            CurrentShape = shape;
        }

        

        // Functions for Particles
        private static readonly Random rng = new();
        protected void OverWriteParticle(int index, Vector2 origin, float size, float Speed, float SpeedDivior=10)
        {
            // Define a new particle

            // Set the position and velocity
            particles.Position[index] = origin;
            particles.Velocity[index] = RandomVelocity(Speed, SpeedDivior);

            // Setting size, age, alpha and alive value
            particles.Size[index] = size;
            particles.Age[index] = 0;
            particles.Alive[index] = true;
            particles.Alpha[index] = 255;

            // Setting rotation to 0
            particles.Rotation[index] = 0;

        }

        private static Vector2 RandomVec = Vector2.Zero;
        public static Vector2 RandomVelocity(float Speed, float SpeedDivisor)
        {
            // Create a new vector
            RandomVec = Vector2.Zero;

            // Creates a Random Direction Vector
            RandomVec.X = (rng.NextSingle() + 0.1f) * 2 - 1;
            RandomVec.Y = (rng.NextSingle() + 0.1f) * 2 - 1;

            // Turns it into a velocity by multiplying by speed
            return Vector2.Normalize(RandomVec) * ((rng.NextSingle() + 0.1f) * (Speed/SpeedDivisor));
        }

        public static void ChangeShape(Shapes shape)
        {
            CurrentShape = shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"> Parameter for where the particle begins to move </param>
        /// <param name="Direction"> The general direction in which the particle moves</param>
        /// <param name="Lifespan"> The time in seconds of the particles lifespan before removal </param>
        /// <param name="StartColor"> The color the particle starts from </param>
        /// <param name="EndColor"> The color the particle 'dies' at </param>/
        /// <param name="Speed"=> The speed in which the particle moves </param>/
        /// <param name="Size_Change"=> By how much in bytes the fade is changing by </param>/
        /// <param name="Fade_Change"=> By how much in bytes the fade is changing by </param>/
        private static int i; 
        private static float factor; // for getting the ratio, factor for lerp
        private static Color particleColor; 
        public void UpdateParticles(
            Vector2 origin, Vector2 Direction, int Lifespan, Color StartColor, Color EndColor, 
            float Size, float DirectionFactor, float Size_Change, byte Fade_Change, float SpeedDivior=10)
        {
            for (i = 0; i < particles.Position.Length-1; i++)
            {
                // Check if the particle is Alive
                if (particles.Alive[i] == true)
                {
                    // Update the Position with Velocity
                    particles.Velocity[i] += Direction * DirectionFactor * Raylib.GetFrameTime(); // make the velocity
                    particles.Position[i] += particles.Velocity[i] * Raylib.GetFrameTime();

                    // Change the particle size
                    particles.Size[i] -= Size_Change;

                    // Increasing the particle age and checking if its less than the lifespan
                    particles.Age[i] += 1; // increase 

                    // 'kill' the particle if it exceeds the lifespan
                    if (particles.Age[i] > Lifespan*Raylib.GetFPS()) particles.Alive[i] = false;

                    // Set the color to the interpolation of the Start and End Color
                    // The factor is the ratio of the age to the lifespan
                    factor = particles.Age[i] / (Lifespan*Raylib.GetFPS());
                    particleColor = Raylib.ColorLerp(StartColor, EndColor, factor);

                    // Make the color of the particle fade
                    // Particle will die if fade is too small
                    particles.Alpha[i] -= Fade_Change;
                    if (particles.Alpha[i] < 10) particles.Alive[i] = false;
                    particleColor.A = particles.Alpha[i];

                    // Rotating the particle
                    particles.Rotation[i] += 5; 

                }
                // The particle is dead
                else if (particles.Alive[i] == false && rng.NextSingle() < 0.1f)
                {
                    // Overwrite the particle
                    OverWriteParticle(i, origin, Size, DirectionFactor, SpeedDivior);
                }

                // Drawing the particle
                Raylib.DrawPoly(particles.Position[i], ShapeSides[CurrentShape], particles.Size[i], particles.Rotation[i], particleColor);
            }
        }


    }
}