# Dating App

_Curso de parendizaje Dotnet + Angular_

## Comenzando 🚀

_El backend o API se desarrolla sobre Dotnet bajo el lenguaje C#. Y para el frontend y conexion con la api se utiliza Angular; bajo el lenguaje Typescript_



## Mensajes ⚙️
_Inclusión de envio de mensajes dentro de la web, desde el backend de la api hasta a nivel frontend Angular_
_-API:_
```
Primero añadimos la clase Entity, que representa a nuestra clase Mensaje en el dominio.

Despues creamos las tablas en la base SQL con la clase DataContext, con sus respectivas entities en el constructor, "builder"

Para el caso de los mensajes incluimos un par de arrays de argumentos a la clase AppUser, ya que el usuario debe tener mensajes enviados y recibidos.

A continuación creamos su interfaz para su repositorio de mensajes, y con ello cremos su MessageRepository que implementar los metodos
de añadir o eliminar mensajes usando el Datacontext context en su constructor. Añadir que para el envio como parametro de los mensajes 
creamos previamente un MessageDto, que tiene el mismo contenido pero solo se utiliza en los metodos get y set del Repositorio para
guardar bien los datos en el envio cuando se les llame desde el frontend.

Por último es añadir el servicio en la extensión, es decir, en ApplicationServiceExtensions  añadir un services.AddScoped para la interfaz
y el repositorio de mensajes. Con esto tenemos lo basico para las entidades de mensajes.

Ahora debemos crear una relación entre la Entity Message y su dto MessageDto, para ello usamos el autoMapper, un map entre ambas clases.
En el caso de contener fotos hay que buscarlas usando el metodo FirstOrDefault(IsMain).Url para encontrarlas.

Creamos otro dto para la creación de mensajes en especifico. Y pasamos a la parte clave: CREAMOS EL CONTROLADOR DE MENSAJES: MessagesController
que engloba los metodos de acceso a la API. A parte como suplemento a mayores del Message Entity, añadimos el helper MessageParams, usado en 
los metodos de obtención del mensaje e hilo de mensajes con el que obtener el usuario y comprobar su contenido.
```
_-ANGULAR_



## Despliegue 📦

_Agrega notas adicionales sobre como hacer deploy_

## Construido con 🛠️

_Menciona las herramientas que utilizaste para crear tu proyecto_

* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - El framework web usado
* [Maven](https://maven.apache.org/) - Manejador de dependencias
* [ROME](https://rometools.github.io/rome/) - Usado para generar RSS

## Contribuyendo 🖇️

Por favor lee el [CONTRIBUTING.md](https://gist.github.com/villanuevand/xxxxxx) para detalles de nuestro código de conducta, y el proceso para enviarnos pull requests.

## Wiki 📖

Puedes encontrar mucho más de cómo utilizar este proyecto en nuestra [Wiki](https://github.com/tu/proyecto/wiki)

## Versionado 📌

Usamos [SemVer](http://semver.org/) para el versionado. Para todas las versiones disponibles, mira los [tags en este repositorio](https://github.com/tu/proyecto/tags).

## Autores ✒️

_Menciona a todos aquellos que ayudaron a levantar el proyecto desde sus inicios_

* **Andrés Villanueva** - *Trabajo Inicial* - [villanuevand](https://github.com/villanuevand)
* **Fulanito Detal** - *Documentación* - [fulanitodetal](#fulanito-de-tal)

También puedes mirar la lista de todos los [contribuyentes](https://github.com/your/project/contributors) quíenes han participado en este proyecto. 

## Licencia 📄

Este proyecto está bajo la Licencia (Tu Licencia) - mira el archivo [LICENSE.md](LICENSE.md) para detalles

## Expresiones de Gratitud 🎁

* Comenta a otros sobre este proyecto 📢
* Invita una cerveza 🍺 o un café ☕ a alguien del equipo. 
* Da las gracias públicamente 🤓.
* etc.

