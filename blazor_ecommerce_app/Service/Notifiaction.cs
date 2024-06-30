namespace blazor_ecommerce_app.Service.Notification
{
    public class ToastOption
    {
        public Exception Exception { get; set; }
        public string Content { get; set; }
        public ToastType Type { get; set; }
        public string css { get { return ToastService.DajKlaseCss(Type); } private set { } }
    }

    public enum ToastType
    {
        error = 1,
        succes = 2,
        warning = 3,
        info = 4
    }

    public class ToastService
    {
        public event Action<ToastOption> ShowToastTrigger;
        public void ShowToast(ToastOption options)
        {
            if (options.Exception is FluentValidation.ValidationException exception)
            {
                options.Content = string.Join("<br/>", exception.Errors);
            }
            this.ShowToastTrigger.Invoke(options);
        }

        public static string DajKlaseCss(ToastType type)
        {
            if (type == ToastType.error)
                return "e-toast-danger";
            if (type == ToastType.succes)
                return "e-toast-succes";
            if (type == ToastType.info)
                return "e-toast-info";
            if (type == ToastType.warning)
                return "e-toast-warning";
            return "";
        }
    }
}
