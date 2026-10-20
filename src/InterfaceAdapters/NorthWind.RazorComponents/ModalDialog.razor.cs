namespace NorthWind.RazorComponents;

/// <summary>
/// Representa un diálogo modal desacoplado basado en los estilos de Bootstrap 5.
/// </summary>
public partial class ModalDialog : ComponentBase
{
    private readonly string _titleId = $"modal-title-{Guid.NewGuid():N}";

    /// <summary>
    /// Indica si el modal se encuentra visible o cerrado.
    /// Soporta two-way binding con @bind-IsOpen.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento emitido cuando cambia el estado de visibilidad del modal.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Título mostrado en la cabecera predeterminada del modal.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Tamaño del modal (Small, Medium, Large, ExtraLarge).
    /// </summary>
    [Parameter]
    public ModalSize Size { get; set; } = ModalSize.Medium;

    /// <summary>
    /// Determina si al hacer clic en el telón de fondo (backdrop) se cierra el modal.
    /// Por defecto es falso para evitar pérdida accidental de datos.
    /// </summary>
    [Parameter]
    public bool CloseOnBackdropClick { get; set; } = false;

    /// <summary>
    /// Determina si se renderiza el botón de cerrar (X) en el encabezado.
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Centra verticalmente el diálogo modal dentro de la pantalla.
    /// </summary>
    [Parameter]
    public bool Centered { get; set; } = true;

    /// <summary>
    /// Permite el desplazamiento vertical interno en el cuerpo del diálogo si el contenido supera la pantalla.
    /// </summary>
    [Parameter]
    public bool Scrollable { get; set; } = true;

    /// <summary>
    /// Clases CSS aplicadas al encabezado del modal.
    /// </summary>
    [Parameter]
    public string HeaderCssClass { get; set; } = "bg-primary text-white";

    /// <summary>
    /// Plantilla personalizada para el encabezado del modal.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Plantilla personalizada para el cuerpo del modal.
    /// Si es nula, se utiliza ChildContent.
    /// </summary>
    [Parameter]
    public RenderFragment? BodyTemplate { get; set; }

    /// <summary>
    /// Contenido principal por defecto dentro del cuerpo del modal.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Plantilla personalizada para el pie de página del modal (ej. botones de acción).
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary>
    /// Evento invocado cuando el modal se cierra.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Identificador único para el título para cumplimiento de accesibilidad (aria-labelledby).
    /// </summary>
    protected string TitleId => _titleId;

    /// <summary>
    /// Abre el modal de manera programática.
    /// </summary>
    public async Task OpenAsync()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            await IsOpenChanged.InvokeAsync(true);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Cierra el modal de manera programática e invoca los callbacks pertinentes.
    /// </summary>
    public async Task CloseAsync()
    {
        if (IsOpen)
        {
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(false);
            await OnClose.InvokeAsync();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Maneja el clic sobre el telón de fondo exterior.
    /// </summary>
    protected async Task HandleBackdropClickAsync()
    {
        if (CloseOnBackdropClick)
        {
            await CloseAsync();
        }
    }

    /// <summary>
    /// Obtiene las clases CSS de Bootstrap aplicables al contenedor del diálogo modal.
    /// </summary>
    protected string GetDialogClasses()
    {
        var classes = new List<string> { "modal-dialog" };

        switch (Size)
        {
            case ModalSize.Small:
                classes.Add("modal-sm");
                break;
            case ModalSize.Large:
                classes.Add("modal-lg");
                break;
            case ModalSize.ExtraLarge:
                classes.Add("modal-xl");
                break;
        }

        if (Centered)
        {
            classes.Add("modal-dialog-centered");
        }

        if (Scrollable)
        {
            classes.Add("modal-dialog-scrollable");
        }

        return string.Join(" ", classes);
    }
}
