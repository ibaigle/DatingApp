# Dating App

_Curso de parendizaje Dotnet + Angular_

## Comenzando 馃殌

_El backend o API se desarrolla sobre Dotnet bajo el lenguaje C#. Y para el frontend y conexion con la api se utiliza Angular; bajo el lenguaje Typescript_



## Mensajes 馃摝 
_Inclusi贸n de envio de mensajes dentro de la web, desde el backend de la api hasta a nivel frontend Angular_
_-API:_
```
Primero a帽adimos la clase Entity, que representa a nuestra clase Mensaje en el dominio.

Despues creamos las tablas en la base SQL con la clase DataContext, con sus respectivas entities en el constructor, "builder"

Para el caso de los mensajes incluimos un par de arrays de argumentos a la clase AppUser, ya que el usuario debe tener mensajes enviados y recibidos.

A continuaci贸n creamos su interfaz para su repositorio de mensajes, y con ello cremos su MessageRepository que implementar los metodos
de a帽adir o eliminar mensajes usando el Datacontext context en su constructor. A帽adir que para el envio como parametro de los mensajes 
creamos previamente un MessageDto, que tiene el mismo contenido pero solo se utiliza en los metodos get y set del Repositorio para
guardar bien los datos en el envio cuando se les llame desde el frontend.

Por 煤ltimo es a帽adir el servicio en la extensi贸n, es decir, en ApplicationServiceExtensions  a帽adir un services.AddScoped para la interfaz
y el repositorio de mensajes. Con esto tenemos lo basico para las entidades de mensajes.

Ahora debemos crear una relaci贸n entre la Entity Message y su dto MessageDto, para ello usamos el autoMapper, un map entre ambas clases.
En el caso de contener fotos hay que buscarlas usando el metodo FirstOrDefault(IsMain).Url para encontrarlas.

Creamos otro dto para la creaci贸n de mensajes en especifico. Y pasamos a la parte clave: CREAMOS EL CONTROLADOR DE MENSAJES: MessagesController
que engloba los metodos de acceso a la API. A parte como suplemento a mayores del Message Entity, a帽adimos el helper MessageParams, usado en 
los metodos de obtenci贸n del mensaje e hilo de mensajes con el que obtener el usuario y comprobar su contenido.
```
_-ANGULAR_
```
Comenzamos esta parte a帽adiendo una clase a los _models, en la cual metemos messages.ts como interface que indica sus parametros con sus tipos.
Creamos un nuevo servicio dentro de _services con ng g s message ---> incluimos la funcionalidad en el message service creado. Y este servicio se apoya
en unos m茅todos privados que extraemos de la clase auxiliar paginationHelper. En el message.service con la url base incluimos metodos que devuelven 
los mensajes por usuario y sus thread.
Creamos un nuevo componente "messages" en el cual en su Typescript creamos los metodos que cargan los mensajes haciendo uso del message.service. 
Completamos con programar la interfaz en el HTML de messages.
Creamos un nuevo componente dentro de messages=> el member-messages. Dentro de este usamos lo de message.service.ts para su m茅todos. Y completamos
con el template de HTML y el css.
Para cargar bien la pesta帽as de Messages modificamos member-detail, a帽adiendo las memberTabs. Adem谩s aqu铆 incluimos la funcionalidad de member-messages
para el metodo LoadMessages. Y asi toda la funcionalidad de member-messages se quita y pasa a member-detail.
Ahora el valor de member en member-messages pasa a ser de entrada, cambio a mensajes en el Template HTML de member-detail => en click llamar a onTabActivated.
Esto hace que cada pest帽a se pueda llamar con un activated valores= [0,1,2,3]
Para activar la pesta帽as de messages clickando desde el boton: metodo en member-detail =>> este selecciona uno de los valores activated (member-detail.html) y
esto tambien si se clica desde la pesta帽a de tus mensajes. Se a帽ade [queryParams] y se accede desde el html y desde member-detail.ts
El componente member-detail lo cambiamos a su version Static por lo que ahora no hace falta poner una comprobaci贸n IF para mostrarlo en el HTML.
Creamos carpeta _resolvers, con el archivo member-detail.resolver.ts, devolviendo el array de Likes con los miembros que se lo dieron.
--A帽adir funcionalidad de enviar mensaje desde el thread de message detail-- 
Primera a帽adir m茅todo enviarMensaje al servicio meesage.service.ts; el cual se llama desde el member-message.component.ts con su funcionalidad
 y el Front va en el member-message.HTML y cambiar el detalle del mensaje final en member-detail.html
 Ahora el problema viene con las fotos, que al subirlas tiene retardo en cargarla y no la puede devolver hasta que no la consiga.
 Al controlador de mensajes, messagesControlers.cs a帽adimos metodo HttpDelete de message, ademos lo a帽adimos como una condici贸n extra al
 _messageRepository. Con esto ya es a帽adir una comprobacion mas de mensaje eliminado en MessageRepository, y despues se puede a帽adir el metodo eliminar al
 service y al componente.
```


## APS.NET Identity 鈿欙笍

_API_
```
Para esto vamos a usar Role managament, mediante el usao de clases Identity: UserManager<T>, SingInManager<T> y RoleManager<T>
Comenzamos con la Entity AppUser heredando de IdentityUser, crear nueva clase AppRole de IdentityRole, y de esta crear AppUserRole,
para al final crear una colleccion de esta ultima en el AppUser === actua como JOINT TABLE de roles para el usuario 
Se eliminan todas las instancias de en archivos de la API en donde se inicializa la password SALT y se crea una clave de pass HMAC.
----Modificamos el DBContext: primero instalamos el IdentityDBContext desde el NuGet, para aplicarlo en el DataContext:
>>DataContext : IdentityDbContext<AppUser, AppRole, int,IdentityUserClaim<int> ,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>, IdentityUserToken<int>>
y con esto a帽adir el servicio en la extension IdentityServiceExtensions con cada Role.
>>>Creamos nueva Migration en la Database: dotnet ef migrations add IdentityAdded <==== Lo ultimo su nombre, y actualizamos el metodo seed con UserManager
>>>Dropeamos las tablas con: dotnet ef database drop . Y al volver a correrlo con dotnet watch run lo creara con la nueva UserManager.
En AccountController se incluye UserManager y singInManager para reemplazar al DataContext. 
Para incluir roles a los usuarios, usamos la claso AppRole mediante RoleManager, esto se hace en Seed.cs para a帽adir un nuevo "role" 
para cada user seg煤n el base "member", y luego se le de "admin" o "moderator", y esto sea ejecutado desde la llamada al seed de Program.cs
A帽adimos los roles al JWT Token ==> tokenService; a帽adiendole el userManager, del que obtenemos los roles y se a帽aden a la lista Claims.
Esto hace que tengamos que ponerlo en el accountController como async(await). Y con estos roles ya podemos crear sus controladores:
>AdminControler: para cada metodo solo se permite una Policy estilo: RequireAdminRole, y hay que crearlas en IdentityServiceExtensions
```
_ANGULAR_
```
Vamos a client/src/app y creamos una carpeta para el componente de  Admin, y ejecutamos: ng g c admin-panel --skip-tests
Para crear la ruta especifica para este componente ---> app-routing.module.ts y pego esta linea: {path: 'admin', component: AdminPanelComponent}, en Routes
Ponemos su nueva pesta帽a en el html doblando un <li>. A帽adimios m茅todo getDecoded a Account.service.ts, hacemos que lo incie cuando se llama
al m茅todo de obtener el user e inicialice. En _guards ejecuto: ng g guard admin --skip-tests => obtengo admin.guard.ts
Directorio nuevo _directives => ng g -h  y despues => ng g d has-role --skip-tests: crea directiva has-role y la a帽ade a app.module.ts
Creamos 2 componentes, el userManagement y el photoManagement, y con el directive hasRole, si el role es Admin muestra el UserManagement o Moderator el photoManagement en el html
Ahora en _services creamos el nuevo para Admin.service.ts, con el metodo que devuelve los UsersWithRoles, el cual se llama desde el user-management.component.ts y su HTML.
A帽adimos ModalModule en el "shared.module.ts": su funcionalidad de implementa con la clase BsModalRef en roles-modal.component.ts y roles-modal
Su implementaci贸n se basa en a帽adir un selector con el que editar los roles desde la ventana admin. Incluyente sus componente graficos obtenidos de ngx-bootstrap web.
```

## Unit of Work 鈿欙笍

La Unidad de Trabajo inyecta instancias de Data Context que se llaman desde todos los repositorios (llamados por los controladores)  de forma responsiva; quiere decir cuando se piden.
_API_
```
Primero la interfaz de UnitOfWork y despues su clase de dominio dentro de la carpeta Data, llamada UnitOfWork.cs
En esta clase se inicializan User, Message y Likes Repository. Y con esto se pueden quitar los services de Likes y Message de ApplicationServiceExtension y 
sustituirlos por <IUnitOfWork, UnitOf Work>. Despues quitamos todos los m茅todos SaveAllAsync de UserRepository y MessageRepository y de sus Interfaces.
Tras esto hay que modificar todos los controladores por remplazar usuario y likes repository por sus vaariables en el UnitOfWork.
```



## Construido con 馃洜锔?

_Menciona las herramientas que utilizaste para crear tu proyecto_

* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - El framework web usado
* [Maven](https://maven.apache.org/) - Manejador de dependencias
* [ROME](https://rometools.github.io/rome/) - Usado para generar RSS

## Contribuyendo 馃枃锔?

Por favor lee el [CONTRIBUTING.md](https://gist.github.com/villanuevand/xxxxxx) para detalles de nuestro c贸digo de conducta, y el proceso para enviarnos pull requests.

## Wiki 馃摉

Puedes encontrar mucho m谩s de c贸mo utilizar este proyecto en nuestra [Wiki](https://github.com/tu/proyecto/wiki)

## Versionado 馃搶

Usamos [SemVer](http://semver.org/) para el versionado. Para todas las versiones disponibles, mira los [tags en este repositorio](https://github.com/tu/proyecto/tags).

## Autores 鉁掞笍

_Menciona a todos aquellos que ayudaron a levantar el proyecto desde sus inicios_

* **Andr茅s Villanueva** - *Trabajo Inicial* - [villanuevand](https://github.com/villanuevand)
* **Fulanito Detal** - *Documentaci贸n* - [fulanitodetal](#fulanito-de-tal)

Tambi茅n puedes mirar la lista de todos los [contribuyentes](https://github.com/your/project/contributors) qu铆enes han participado en este proyecto. 

## Licencia 馃搫

Este proyecto est谩 bajo la Licencia (Tu Licencia) - mira el archivo [LICENSE.md](LICENSE.md) para detalles

## Expresiones de Gratitud 馃巵

* Comenta a otros sobre este proyecto 馃摙
* Invita una cerveza 馃嵑 o un caf茅 鈽? a alguien del equipo. 
* Da las gracias p煤blicamente 馃.
* etc.

