using LoanManagementSystem.Data;
using LoanManagementSystem.Interfaces;
using LoanManagementSystem.Managers;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Services;

/// <summary>
/// Simple Service Locator pattern implementation.
/// Provides centralized dependency resolution for the application.
/// 
/// In a larger application, consider using Microsoft.Extensions.DependencyInjection
/// or another DI container.
/// 
/// Design Pattern: Service Locator
/// SOLID: Supports Dependency Inversion Principle
/// </summary>
public sealed class ServiceLocator
{
    private static ServiceLocator? _instance;
    private static readonly object _lock = new();
    
    private readonly Dictionary<Type, Func<object>> _services = [];
    private readonly Dictionary<Type, object> _singletons = [];
    
    private ServiceLocator() { }
    
    /// <summary>
    /// Gets the singleton instance of the ServiceLocator.
    /// Thread-safe implementation using double-checked locking.
    /// </summary>
    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new ServiceLocator();
                }
            }
            return _instance;
        }
    }
    
    /// <summary>
    /// Registers a factory function for creating instances of a service type.
    /// </summary>
    public void Register<TInterface, TImplementation>(Func<TImplementation> factory) 
        where TImplementation : class, TInterface
        where TInterface : class
    {
        _services[typeof(TInterface)] = factory;
    }
    
    /// <summary>
    /// Registers a singleton instance for a service type.
    /// </summary>
    public void RegisterSingleton<TInterface>(TInterface instance) 
        where TInterface : class
    {
        _singletons[typeof(TInterface)] = instance;
    }
    
    /// <summary>
    /// Registers a factory function that will create a singleton on first resolution.
    /// </summary>
    public void RegisterLazySingleton<TInterface, TImplementation>(Func<TImplementation> factory)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        _services[typeof(TInterface)] = () =>
        {
            if (!_singletons.ContainsKey(typeof(TInterface)))
            {
                _singletons[typeof(TInterface)] = factory();
            }
            return _singletons[typeof(TInterface)];
        };
    }
    
    /// <summary>
    /// Resolves a service by its interface type.
    /// </summary>
    public TInterface Resolve<TInterface>() where TInterface : class
    {
        // Check singletons first
        if (_singletons.TryGetValue(typeof(TInterface), out var singleton))
        {
            return (TInterface)singleton;
        }
        
        // Check factory registrations
        if (_services.TryGetValue(typeof(TInterface), out var factory))
        {
            return (TInterface)factory();
        }
        
        throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} is not registered.");
    }
    
    /// <summary>
    /// Checks if a service is registered.
    /// </summary>
    public bool IsRegistered<TInterface>() where TInterface : class
    {
        return _singletons.ContainsKey(typeof(TInterface)) || _services.ContainsKey(typeof(TInterface));
    }
    
    /// <summary>
    /// Clears all registrations. Useful for testing.
    /// </summary>
    public void Clear()
    {
        _services.Clear();
        _singletons.Clear();
    }
    
    /// <summary>
    /// Configures all application services.
    /// Call this during application startup.
    /// </summary>
    public void ConfigureServices(LoanDbContext context)
    {
        // Register singleton for DbContext
        RegisterSingleton(context);
        
        // Register Session Manager as singleton
        RegisterSingleton<ISessionManager>(new SessionManagerService());
        
        // Register Credit Score Calculator (stateless, can be singleton)
        RegisterSingleton<ICreditScoreCalculator>(new CreditScoreCalculator());
        
        // Register Managers as transient (new instance per request)
        // This is important because they hold references to repositories
        Register<ICustomerManager, CustomerManager>(() => new CustomerManager(context));
        Register<ILoanManager, LoanManager>(() => new LoanManager(context));
        Register<IPaymentManager, PaymentManager>(() => new PaymentManager(context));
        Register<ILoanApplicationManager, LoanApplicationManager>(() => new LoanApplicationManager(context));
        Register<ReportManager, ReportManager>(() => new ReportManager(context));
        Register<LoanProductManager, LoanProductManager>(() => new LoanProductManager(context));
        Register<UserManager, UserManager>(() => new UserManager(context));
    }
}

/// <summary>
/// Implementation of ISessionManager that wraps the static SessionManager.
/// This allows for dependency injection while maintaining backward compatibility.
/// 
/// Design Pattern: Adapter Pattern - Adapts static SessionManager to ISessionManager interface
/// </summary>
public class SessionManagerService : ISessionManager
{
    public User? CurrentUser => SessionManager.CurrentUser;
    public bool IsLoggedIn => SessionManager.IsLoggedIn;
    
    public void Login(User user) => SessionManager.Login(user);
    public void Logout() => SessionManager.Logout();
    public bool HasPermission(Permission permission) => SessionManager.HasPermission(permission);
    public string GetCurrentUsername() => SessionManager.GetCurrentUsername();
    public int GetCurrentUserId() => SessionManager.GetCurrentUserId();
}
