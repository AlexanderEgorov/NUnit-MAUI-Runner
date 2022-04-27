using System;

#pragma warning disable 1591

namespace NUnit.Runner.Annotations {

  [AttributeUsage(
    AttributeTargets.Method | AttributeTargets.Parameter |
    AttributeTargets.Property | AttributeTargets.Delegate |
    AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class CanBeNullAttribute : Attribute { }

  [AttributeUsage(
    AttributeTargets.Method | AttributeTargets.Parameter |
    AttributeTargets.Property | AttributeTargets.Delegate |
    AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class NotNullAttribute : Attribute { }

  [AttributeUsage(
    AttributeTargets.Constructor | AttributeTargets.Method,
    AllowMultiple = false, Inherited = true)]
  public sealed class StringFormatMethodAttribute : Attribute {
      public StringFormatMethodAttribute(string formatParameterName) {
      FormatParameterName = formatParameterName;
    }

    public string FormatParameterName { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public sealed class InvokerParameterNameAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute {
    public NotifyPropertyChangedInvocatorAttribute() { }
    public NotifyPropertyChangedInvocatorAttribute(string parameterName)
    {
      ParameterName = parameterName;
    }

    public string ParameterName { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public sealed class ContractAnnotationAttribute : Attribute {
    public ContractAnnotationAttribute([NotNull] string contract)
      : this(contract, false) { }

    public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
    {
      Contract = contract;
      ForceFullStates = forceFullStates;
    }

    public string Contract { get; private set; }
    public bool ForceFullStates { get; private set; }
  }


  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  public sealed class LocalizationRequiredAttribute : Attribute {
    public LocalizationRequiredAttribute() : this(true) { }
    public LocalizationRequiredAttribute(bool required) {
      Required = required;
    }

    public bool Required { get; private set; }
  }

  [AttributeUsage(
    AttributeTargets.Interface | AttributeTargets.Class |
    AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
  public sealed class CannotApplyEqualityOperatorAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  [BaseTypeRequired(typeof(Attribute))]
  public sealed class BaseTypeRequiredAttribute : Attribute {
    public BaseTypeRequiredAttribute([NotNull] Type baseType) {
      BaseType = baseType;
    }

    [NotNull] public Type BaseType { get; private set; }
  }

  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  public sealed class UsedImplicitlyAttribute : Attribute {
    public UsedImplicitlyAttribute()
      : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

    public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
      : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

    public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
      : this(ImplicitUseKindFlags.Default, targetFlags) { }

    public UsedImplicitlyAttribute(
      ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags) {
      UseKindFlags = useKindFlags;
      TargetFlags = targetFlags;
    }

    public ImplicitUseKindFlags UseKindFlags { get; private set; }
    public ImplicitUseTargetFlags TargetFlags { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public sealed class MeansImplicitUseAttribute : Attribute {
    public MeansImplicitUseAttribute()
      : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

    public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
      : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

    public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
      : this(ImplicitUseKindFlags.Default, targetFlags) { }

    public MeansImplicitUseAttribute(
      ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags) {
      UseKindFlags = useKindFlags;
      TargetFlags = targetFlags;
    }

    [UsedImplicitly] public ImplicitUseKindFlags UseKindFlags { get; private set; }
    [UsedImplicitly] public ImplicitUseTargetFlags TargetFlags { get; private set; }
  }

  [Flags]
  public enum ImplicitUseKindFlags {
    Default = Access | Assign | InstantiatedWithFixedConstructorSignature,
    Access = 1,
    Assign = 2,
    InstantiatedWithFixedConstructorSignature = 4,
    InstantiatedNoFixedConstructorSignature = 8,
  }

  [Flags]
  public enum ImplicitUseTargetFlags {
    Default = Itself,
    Itself = 1,
    Members = 2,
    WithMembers = Itself | Members
  }

  [MeansImplicitUse]
  public sealed class PublicAPIAttribute : Attribute {
    public PublicAPIAttribute() { }
    public PublicAPIAttribute([NotNull] string comment) {
      Comment = comment;
    }

    [NotNull] public string Comment { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
  public sealed class InstantHandleAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Method, Inherited = true)]
  public sealed class PureAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Parameter)]
  public class PathReferenceAttribute : Attribute {
    public PathReferenceAttribute() { }
    public PathReferenceAttribute([PathReference] string basePath) {
      BasePath = basePath;
    }

    [NotNull] public string BasePath { get; private set; }
  }

  // ASP.NET MVC attributes

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute {
    public AspMvcAreaMasterLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcAreaPartialViewLocationFormatAttribute : Attribute {
    public AspMvcAreaPartialViewLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute {
    public AspMvcAreaViewLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcMasterLocationFormatAttribute : Attribute {
    public AspMvcMasterLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcPartialViewLocationFormatAttribute : Attribute {
    public AspMvcPartialViewLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public sealed class AspMvcViewLocationFormatAttribute : Attribute {
    public AspMvcViewLocationFormatAttribute(string format) { }
  }

  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
  public sealed class AspMvcActionAttribute : Attribute {
    public AspMvcActionAttribute() { }
    public AspMvcActionAttribute([NotNull] string anonymousProperty) {
      AnonymousProperty = anonymousProperty;
    }

    [NotNull] public string AnonymousProperty { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcAreaAttribute : PathReferenceAttribute {
    public AspMvcAreaAttribute() { }
    public AspMvcAreaAttribute([NotNull] string anonymousProperty) {
      AnonymousProperty = anonymousProperty;
    }

    [NotNull] public string AnonymousProperty { get; private set; }
  }

  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
  public sealed class AspMvcControllerAttribute : Attribute {
    public AspMvcControllerAttribute() { }
    public AspMvcControllerAttribute([NotNull] string anonymousProperty) {
      AnonymousProperty = anonymousProperty;
    }

    [NotNull] public string AnonymousProperty { get; private set; }
  }


  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcMasterAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcModelTypeAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
  public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute { }


  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public sealed class AspMvcSupressViewErrorAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcDisplayTemplateAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcEditorTemplateAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class AspMvcTemplateAttribute : Attribute { }


  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
  public sealed class AspMvcViewAttribute : PathReferenceAttribute { }


  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
  public sealed class AspMvcActionSelectorAttribute : Attribute { }

  [AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.Property |
    AttributeTargets.Field, Inherited = true)]
  public sealed class HtmlElementAttributesAttribute : Attribute {
    public HtmlElementAttributesAttribute() { }
    public HtmlElementAttributesAttribute([NotNull] string name) {
      Name = name;
    }

    [NotNull] public string Name { get; private set; }
  }

  [AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.Field |
    AttributeTargets.Property, Inherited = true)]
  public sealed class HtmlAttributeValueAttribute : Attribute {
    public HtmlAttributeValueAttribute([NotNull] string name) {
      Name = name;
    }

    [NotNull] public string Name { get; private set; }
  }

  // Razor attributes

  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, Inherited = true)]
  public sealed class RazorSectionAttribute : Attribute { }
}