# Early Explorers

Early Explorers es una aplicación de realidad virtual diseñada para la estimulación temprana de niños, utilizando un visor Meta Quest 1 para la inmersión visual y un Kinect Xbox 360 para el reconocimiento de gestos. El objetivo principal del proyecto es ofrecer un entorno interactivo y educativo en el que los niños puedan explorar, aprender y jugar a través de experiencias lúdicas y accesibles.

## Descripción del Proyecto

El proyecto consiste en un entorno virtual ambientado en un parque de juegos, donde los niños pueden interactuar con diferentes animales y elementos mediante gestos naturales. La aplicación está diseñada para estimular el desarrollo cognitivo y motor, proporcionando una experiencia intuitiva que no requiere el uso de controles físicos, gracias a la integración del Kinect.

### Componentes Principales

1. **Entorno de Realidad Virtual**: Un parque de juegos colorido y amigable, creado para captar la atención de los niños pequeños. Incluye elementos interactivos como columpios, árboles y animales.
   
2. **Interacción con Animales**: El objetivo del juego es que los niños exploren el parque y encuentren animales (actualmente un pollo y un pingüino). Tocando los animales, estos desaparecen, lo que refuerza la curiosidad y la exploración.

3. **Reconocimiento de Gestos**: Utilizando el Kinect Xbox 360 y la SDK 1.8, los niños pueden interactuar con el juego mediante gestos como mover las manos, saltar o realizar un "swipe" para seleccionar y mover objetos dentro del entorno.

4. **Plataforma Tecnológica**: El proyecto se desarrolla en Unity utilizando XRToolkit para la realidad virtual y los Assets de Kinect para la detección de gestos.

## Avances del Proyecto

Menu Interactivo:

Al iniciar se muestra un menú representado con un sistema solar en donde se pueden escoger si es que se desea ingresar el juego como Profesor o como Alumno, dependiendo de lo que se escoja, se da la opción de ingresar a la sala.

Gracias al entorno de red, contamos con dos jugadores que son instanciados según se escoja en el menú y el juego puede iniciar.

Tutorial:

Dependiendo del jugador que se escoja (Profesor o Alumno), se muestra un video en un panel que da a conocer un tutorial y de esa forma enseña a cada jugador que debe hacer dentro del juego por separado.

Primer Jugador (Profesor):

En cuanto a la funcionalidad dell profesor tiene la opción de crear un camino con las flechas del teclado, el camino esta hecho con pathNodes y esta marcado de color rojo para que se aprecie la visualizacion, solo el jugador que ingrese como profesor y tiene ese ROL sera capaz de crear un camino, el alumno no, la camara del profesor se encuentra situada en la parte superior de la escena para que tenga una mejor visión de donde crear los caminos.

Segundo Jugador (Alumno):

El Alumno puede seguir el camino creado por el profesor en tiempo real mediante gestos que son reconocidos por el kinect como levantar la mano para moverse hacia adelante o hacer swipe left y right para poder girar, el Alumno no puede salir de unos límites puestos en el camino para que así la funcionalidad del juego esté intacta.



## Próximos Pasos

- Realizar pruebas con usuarios reales (niños) para obtener feedback y mejorar la interacción y accesibilidad.
- Integrar de forma completa las funcionalidades de Kinect y VR para permitir interacciones más complejas dentro del entorno.
- Mejorar la interacción con los animales y optimizar la precisión de los gestos para una experiencia más natural.

## Requisitos

Para ejecutar el proyecto, se necesita:

- [Meta Quest 1](https://www.meta.com/quest/) (o visor compatible con realidad virtual).
- [Kinect Xbox 360](https://www.xbox.com/accessories/kinect) con Kinect SDK 1.8.
- [Unity 2020.3](https://unity.com/releases/2020-lts) o superior.
- Assets y herramientas de realidad virtual (XRToolkit) y Kinect disponibles en Unity Asset Store.

## Instalación

1. Clona este repositorio:
   ```bash
   git clone https://github.com/tu-usuario/early-explorers.git
2.Abre el proyecto en Unity.

3.Asegúrate de tener configurados los controladores de Meta Quest 1 y Kinect SDK 1.8.

4.Ejecuta el proyecto en modo de realidad virtual.

# Video del proyecto

Puedes ver una demostración del proyecto en el siguiente enlace:

[Ver Video en Google Drive](https://drive.google.com/file/d/1BksQrKnwkmK6aZj_Rn1IdpJdGiKr3qGI/view?usp=sharing)


