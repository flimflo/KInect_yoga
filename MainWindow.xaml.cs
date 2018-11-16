/*
 Kinect_yoga
 Fernando Limon Flores
 Jose Emiliano Gonzalez Jimenez
 V 1.0
 27/Sep/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Threading;
using System.Windows.Media;

namespace Kinect_yoga
{
    /// <summary>
    /// Capítulo: Reflejar el movimiento con imágenes
    /// Ejemplo: Obtener la posición de la mano derecha (De cualquier persona, no se selecciona cual)
    /// Descripción: 
    ///              Este sencillo ejemplo muestra una ventana con un círculo del cual, su movimiento, refleja el 
    ///              movimiento de la mano derecha. Conforme se mueve la mano se mueve el círculo.
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer1 = new DispatcherTimer();
        int tiempo = 4;
        int tiempo1 = 0;
        bool Pase = true;
        bool Pase1 = true;
        bool Pase2 = true;
        int puntos = 0;
        DispatcherTimer timer2 = new DispatcherTimer();
        DispatcherTimer timer3 = new DispatcherTimer();
        int tiempo2 = 4;
        int tiempo3 = 0;

        private KinectSensor miKinect;  //Representa el Kinect conectado

        /* ----------------------- Área para las variables ------------------------- */
        private WriteableBitmap imagen; //Se utiliza para generar la imagen a partir del arreglo de bytes recibidos
        private byte[] cantidadPixeles; //Arreglo para recibir los bytes que envía el Kinect
        /* ------------------------------------------------------------------------- */

        double dMano_X;            //Representa la coordenada X de la mano derecha
        double dMano_Y;            //Representa la coordenada Y de la mano derecha
        //////////////////////////////////////////////////////////////////////////
        double iMano_X;             //Representa la coordenada X de la mano izquierda
        double iMano_Y;             //Representa la coordenada Y de la mano izquierda
        //////////////////////////////////////////////////////////////////////////
        double Cabeza_X;             //Representa la coordenada X de la cabeza
        double Cabeza_Y;             //Representa la coordenada Y de la cabeza
        //////////////////////////////////////////////////////////////////////////
        double dRodilla_X;             //Representa la coordenada X de la rodilla derecha
        double dRodilla_Y;             //Representa la coordenada Y de la rodilla derecha
        //////////////////////////////////////////////////////////////////////////
        double iRodilla_X;             //Representa la coordenada X de la rodilla izquierda
        double iRodilla_Y;             //Representa la coordenada Y de la rodilla izquierda
        //////////////////////////////////////////////////////////////////////////
        double dPie_X;             //Representa la coordenada X del pie derecho
        double dPie_Y;             //Representa la coordenada Y del pie derecho
        //////////////////////////////////////////////////////////////////////////
        double iPie_X;             //Representa la coordenada X del pie izquierdo
        double iPie_Y;             //Representa la coordenada Y del pie izquierdo

        Point joint_Point = new Point(); //Permite obtener los datos del Joint
        //Variables	que	se	emplearán	para	almacenar	el	centro	del	aro
        double dXC, dYC;
        double iXC, iYC;
        double CXC, CYC;
        //Variables	que	almacenan	el	radio	de	cada	uno	de	los	círculos.
        double dRadioC1, dRadioC2;
        public MainWindow()
        {
            InitializeComponent();

            Kinect_Config();

            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick);

            timer1.Interval = new TimeSpan(0, 0, 0, 1);
            timer1.Tick += new EventHandler(Barra_prog);

            timer2.Interval = new TimeSpan(0, 0, 0, 1);
            timer2.Tick += new EventHandler(Timer_Tick1);

            timer3.Interval = new TimeSpan(0, 0, 0, 1);
            timer3.Tick += new EventHandler(Barra_prog1);
        }

        private void Barra_prog(object sender, EventArgs e)
        {
            tiempo1++;
            Progreso.Value = (double)tiempo1;
            if(tiempo1 == Progreso.Maximum)
            {
                Pase = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tiempo--;
            Temporizador.Content = tiempo.ToString();

            if (tiempo < 0)
            {
                Temporizador.Visibility = Visibility.Hidden;
                timer1.IsEnabled = true;
            }
        }

        private void Barra_prog1(object sender, EventArgs e)
        {
            tiempo3++;
            Progreso1.Value = (double)tiempo3;
            if (tiempo3 == Progreso1.Maximum)
            {
                Pase2 = false;
            }
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            tiempo2--;
            Temporizador1.Content = tiempo2.ToString();

            if (tiempo2 < 0)
            {
                Temporizador1.Visibility = Visibility.Hidden;
                timer3.IsEnabled = true;
            }
        }

        private void usarCamara()
        {
            // Escribir los datos en el Bitmap
            this.imagen.WritePixels(new Int32Rect(0, 0, this.imagen.PixelWidth, this.imagen.PixelHeight), this.cantidadPixeles, this.imagen.PixelWidth * sizeof(int), 0);
        }

        /* -- Área para el método que utiliza los datos proporcionados por Kinect -- */
        /// <summary>
        /// Método que realiza las manipulaciones necesarias sobre el Skeleton trazado
        /// </summary>
        private void usarSkeleton(Skeleton skeleton)
        {
            Joint joint1 = skeleton.Joints[JointType.HandRight];
            /////////////
            Joint joint2 = skeleton.Joints[JointType.HandLeft];
            ////////////
            ////////////
            Joint joint3 = skeleton.Joints[JointType.Head];
            ////////////
            ////////////
            Joint joint4 = skeleton.Joints[JointType.KneeRight];
            ////////////
            ////////////
            Joint joint5 = skeleton.Joints[JointType.KneeLeft];
            ////////////
            ////////////
            Joint joint6 = skeleton.Joints[JointType.FootRight];
            ////////////
            ////////////
            Joint joint7 = skeleton.Joints[JointType.FootLeft];
            ////////////
            //	Si	el	Joint	está	listo	obtener	las	coordenadas
            if (joint1.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint1.Position);
                dMano_X = joint_Point.X;
                dMano_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero.SetValue(Canvas.TopProperty, dMano_Y+35);
                Puntero.SetValue(Canvas.LeftProperty, dMano_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////
            if (joint2.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint2.Position);
                iMano_X = joint_Point.X;
                iMano_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero1.SetValue(Canvas.TopProperty, iMano_Y+35);
                Puntero1.SetValue(Canvas.LeftProperty, iMano_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////////////////
            if (joint3.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint3.Position);
                Cabeza_X = joint_Point.X;
                Cabeza_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero3.SetValue(Canvas.TopProperty, Cabeza_Y+35);
                Puntero3.SetValue(Canvas.LeftProperty, Cabeza_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////////////////
            //	Si	el	Joint	está	listo	obtener	las	coordenadas
            if (joint4.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint4.Position);
                dRodilla_X = joint_Point.X;
                dRodilla_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero4.SetValue(Canvas.TopProperty, dRodilla_Y+35);
                Puntero4.SetValue(Canvas.LeftProperty, dRodilla_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////
            //	Si	el	Joint	está	listo	obtener	las	coordenadas
            if (joint5.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint5.Position);
                iRodilla_X = joint_Point.X;
                iRodilla_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero5.SetValue(Canvas.TopProperty, iRodilla_Y+35);
                Puntero5.SetValue(Canvas.LeftProperty, iRodilla_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;  
            }
            ////////////////
            //	Si	el	Joint	está	listo	obtener	las	coordenadas
            if (joint6.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint6.Position);
                dPie_X = joint_Point.X;
                dPie_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero6.SetValue(Canvas.TopProperty, dPie_Y+35);
                Puntero6.SetValue(Canvas.LeftProperty, dPie_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////
            //	Si	el	Joint	está	listo	obtener	las	coordenadas
            if (joint7.TrackingState == JointTrackingState.Tracked)
            {
                //	Obtiene	las	coordenadas	(x,	y)	del	Joint
                joint_Point = this.SkeletonPointToScreen(joint7.Position);
                iPie_X = joint_Point.X;
                iPie_Y = joint_Point.Y;
                //Emplea	las	coordenadas	del	Joint	para	mover	la	elipse	
                Puntero7.SetValue(Canvas.TopProperty, iPie_Y+35);
                Puntero7.SetValue(Canvas.LeftProperty, iPie_X+30);
                //	Obtiene	el	Id	de	la	persona	mapeada
                LID.Content = skeleton.TrackingId;
            }
            ////////////////

            if (Posicion() && Pase)
            {
                EscPunte(true);
                Bandera.Fill = Brushes.Green;
                MainCanvas.Background = Brushes.Transparent;
                Pos1.Height = 200;
                Pos1.SetValue(Canvas.TopProperty, 325.0);
                Pos1.SetValue(Canvas.LeftProperty, 600.0);
                timer.IsEnabled = true;
                puntos++;
            }
            else if(Pase) 
            {
                Bandera.Fill = Brushes.Red;
            }
            else if(Pase1)
            {
                EscPunte(false);
                Bandera.Fill = Brushes.Red;
                MainCanvas.Background = Brushes.White;
                Pos1.Visibility = Visibility.Hidden;
                Pos2.Visibility = Visibility.Visible;
                Progreso.Value = 0.0;
                Progreso.Visibility = Visibility.Hidden;
                Pase1 = false;
            }
            else if(Posicion1() && Pase2)
            {
                EscPunte(true);
                Bandera.Fill = Brushes.Green;
                MainCanvas.Background = Brushes.Transparent;
                Pos2.Height = 200;
                Pos2.SetValue(Canvas.TopProperty, 325.0);
                Pos2.SetValue(Canvas.LeftProperty, 425.0);
                timer2.IsEnabled = true;
                puntos++;
            }else if (Pase2)
            {
                Bandera.Fill = Brushes.Red;
            }
            else
            {
                EscPunte(false);
                Bandera.Fill = Brushes.Black;
                MainCanvas.Background = Brushes.Black;
                Pos2.Visibility = Visibility.Hidden;
                Progreso1.Visibility = Visibility.Hidden;
                Puntuacion.Content = "Tu puntuacion es: " + puntos.ToString();
            }


        }
        /* ------------------------------------------------------------------------- */

        /* --------------------------- Métodos Nuevos ------------------------------ */


        private bool Posicion()
        {
            double Manod = (double)Puntero.GetValue(Canvas.TopProperty);
            double Cabeza = (double)Puntero3.GetValue(Canvas.TopProperty);
            double Manoi = (double)Puntero1.GetValue(Canvas.TopProperty);
            double Rodillai = (double)Puntero5.GetValue(Canvas.TopProperty);
            double Pied = (double)Puntero6.GetValue(Canvas.TopProperty);

            if (Manod < Cabeza && Manoi < Cabeza && Rodillai + 50 > Pied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Posicion1()
        {
            
            double Manod = (double)Puntero.GetValue(Canvas.TopProperty);
            double Cabeza = (double)Puntero3.GetValue(Canvas.TopProperty);
            double Manoi = (double)Puntero1.GetValue(Canvas.TopProperty);
            double Rodillai = (double)Puntero5.GetValue(Canvas.TopProperty);
            double Rodillad = (double)Puntero4.GetValue(Canvas.TopProperty);
            //double Pied = (double)Puntero6.GetValue(Canvas.TopProperty);

            if (Manod < Cabeza && Manoi < Cabeza && (Rodillai > Rodillad + 15))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void EscPunte(bool indicador)
        {
            if (indicador)
            {
                Puntero.Visibility = Visibility.Hidden;
                Puntero1.Visibility = Visibility.Hidden;
                Puntero3.Visibility = Visibility.Hidden;
                Puntero4.Visibility = Visibility.Hidden;
                Puntero5.Visibility = Visibility.Hidden;
                Puntero6.Visibility = Visibility.Hidden;
                Puntero7.Visibility = Visibility.Hidden;
            }
            else
            {
                Puntero.Visibility = Visibility.Visible;
                Puntero1.Visibility = Visibility.Visible;
                Puntero3.Visibility = Visibility.Visible;
                Puntero4.Visibility = Visibility.Visible;
                Puntero5.Visibility = Visibility.Visible;
                Puntero6.Visibility = Visibility.Visible;
                Puntero7.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Metodo que convierte un "SkeletonPoint" a "DepthSpace", esto nos permite poder representar las coordenadas de los Joints
        /// en nuestra ventana en las dimensiones deseadas.
        /// </summary>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convertertir un punto a "Depth Space" en una resolución de 640x480
            DepthImagePoint depthPoint = this.miKinect.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
        /* ------------------------------------------------------------------------- */

        /// <summary>
        /// Método que realiza las configuraciones necesarias en el Kinect 
        /// así también inicia el Kinect para el envío de datos
        /// </summary>
        private void Kinect_Config()
        {
            // Buscamos el Kinect conectado con la propiedad KinectSensors, al descubrir el primero con el estado Connected
            // se asigna a la variable miKinect que lo representará (KinectSensor miKinect)
            miKinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);

            if (this.miKinect != null && !this.miKinect.IsRunning)
            {

                /* ------------------- Configuración del Kinect ------------------- */
                // Habilitar el SkeletonStream para permitir el trazo de "Skeleton"
                this.miKinect.SkeletonStream.Enable();

                // Enlistar al evento que se ejecuta cada vez que el Kinect tiene datos listos
                this.miKinect.SkeletonFrameReady += this.Kinect_FrameReady;
                /* ---------------------------------------------------------------- */

                /* ------------------- Configuración del Kinect ------------------- */
                // Habilitar ColorStream con una resolución de 640x480 a una razón de 30 frames / seg
                this.miKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Enlistar la función que se llamará cada vez que el Kinect tiene listo un frame de datos
                this.miKinect.ColorFrameReady += this.Kinect_FrameReady1;

                // Crear el arreglo que recibe los datos de los pixeles, FramePixelDataLength es el número de bytes en el frame
                this.cantidadPixeles = new byte[this.miKinect.ColorStream.FramePixelDataLength];

                // Crear el WriteableBitmap que tendrá la imagen
                this.imagen = new WriteableBitmap(this.miKinect.ColorStream.FrameWidth, this.miKinect.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Asignar la imagen como fuente para ser mostrada en la ventana
                this.Image.Source = this.imagen;
                /* ---------------------------------------------------------------- */

                // Enlistar el método que se llama cada vez que hay un cambio en el estado del Kinect
                KinectSensor.KinectSensors.StatusChanged += Kinect_StatusChanged;

                // Iniciar el Kinect
                try
                {
                    this.miKinect.Start();
                }
                catch (IOException)
                {
                    this.miKinect = null;
                }
                LEstatus.Content = "Conectado";
            }
            else
            {
                // Enlistar el método que se llama cada vez que hay un cambio en el estado del Kinect
                KinectSensor.KinectSensors.StatusChanged += Kinect_StatusChanged;
            }
        }
        /// <summary>
        /// Método que adquiere los datos que envia el Kinect, su contenido varía según la tecnología 
        /// que se esté utilizando (Cámara, SkeletonTraking, DepthSensor, etc)
        /// </summary>
        private void Kinect_FrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            // Arreglo que recibe los datos  
            Skeleton[] skeletons = new Skeleton[0];
            Skeleton skeleton;

            // Abrir el frame recibido y copiarlo al arreglo skeletons
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            // Seleccionar el primer Skeleton trazado
            skeleton = (from trackSkeleton in skeletons where trackSkeleton.TrackingState == SkeletonTrackingState.Tracked select trackSkeleton).FirstOrDefault();

            if (skeleton == null)
            {
                LID.Content = "0";
                return;
            }
            LID.Content = skeleton.TrackingId;

            // Enviar el Skelton a usar
            this.usarSkeleton(skeleton);
        }

        /// <summary>
        /// Método que adquiere los datos que envia el Kinect, su contenido varía según la tecnología 
        /// que se esté utilizando (Cámara, SkeletonTraking, DepthSensor, etc)
        /// </summary>
        private void Kinect_FrameReady1(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copiar los datos(referentes a los pixeles) del frame a un arreglo
                    colorFrame.CopyPixelDataTo(this.cantidadPixeles);

                    // Manipular los bytes en el arreglo
                    usarCamara();
                }
            }
        }
        /// <summary>
        /// Método que configura del Kinect de acuerdo a su estado(conectado, desconectado, etc),
        /// su contenido varia según la tecnología que se esté utilizando (Cámara, SkeletonTraking, DepthSensor, etc)
        /// </summary>
        private void Kinect_StatusChanged(object sender, StatusChangedEventArgs e)
        {

            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (this.miKinect == null)
                    {
                        this.miKinect = e.Sensor;
                    }

                    if (this.miKinect != null && !this.miKinect.IsRunning)
                    {
                        /* ------------------- Configuración del Kinect ------------------- */
                        // Habilitar el SkeletonStream para permitir el trazo de "Skeleton"
                        this.miKinect.SkeletonStream.Enable();

                        // Enlistar al evento que se ejecuta cada vez que el Kinect tiene datos listos
                        this.miKinect.SkeletonFrameReady += this.Kinect_FrameReady;
                        /* ---------------------------------------------------------------- */

                        /* ------------------- Configuración del Kinect ------------------- */
                        //Habilitar ColorStream con una resolución de 640x480 a una razón de 30 frames/seg
                        this.miKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                        //Enlistar la función que se llamará cada vez que el Kinect tenga listo un frame de datos
                        this.miKinect.ColorFrameReady += this.Kinect_FrameReady1;

                        // Crear el arreglo que recibe los datos de los pixeles, FramePixelDataLength es el número de bytes en el frame
                        this.cantidadPixeles = new byte[this.miKinect.ColorStream.FramePixelDataLength];

                        // Crear el WriteableBitmap que tendrá la imagen
                        this.imagen = new WriteableBitmap(this.miKinect.ColorStream.FrameWidth, this.miKinect.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                        // Asignar la imagen como fuente para ser mostrada en la ventana
                        this.Image.Source = this.imagen;
                        /* ---------------------------------------------------------------- */

                        // Iniciar el Kinect
                        try
                        {
                            this.miKinect.Start();
                        }
                        catch (IOException)
                        {
                            this.miKinect = null;
                        }
                        LEstatus.Content = "Conectado";
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (this.miKinect == e.Sensor)
                    {
                        /* ------------------- Configuración del Kinect ------------------- */
                        this.miKinect.SkeletonFrameReady -= this.Kinect_FrameReady;
                        /* ---------------------------------------------------------------- */

                        /* ------------------- Configuración del Kinect ------------------- */
                        this.miKinect.ColorFrameReady -= this.Kinect_FrameReady1;
                        /* ---------------------------------------------------------------- */

                        this.miKinect.Stop();
                        this.miKinect = null;
                        LEstatus.Content = "Desconectado";

                    }
                    break;
            }
        }
        /// <summary>
        /// Método que libera los recursos del Kinect cuando se termina la aplicación
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.miKinect != null && this.miKinect.IsRunning)
            {
                /* ------------------- Configuración del Kinect ------------------- */
                this.miKinect.SkeletonFrameReady -= this.Kinect_FrameReady;
                /* ---------------------------------------------------------------- */

                /* ------------------- Configuración del Kinect ------------------- */
                this.miKinect.ColorFrameReady -= this.Kinect_FrameReady1;
                /* ---------------------------------------------------------------- */

                this.miKinect.Stop();
            }
        }
    }
}