using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public abstract class BaseInformationManager
    {
        protected async Task<Domain.Associated<T>> LoadAssociatedDomainObject<T>(
            Domain.Associated<T>.LoadingType desiredLoadingType,
            Func<Task<Domain.Associated<T>>> loadDomainObjectAsReference,
            Func<Domain.Associated<T>> loadDomainObjectAsForeignKey = null)
            where T : Domain.DomainObjectBase
        {
            switch (desiredLoadingType)
            {
                case Domain.Associated<T>.LoadingType.None:
                    return null;
                case Domain.Associated<T>.LoadingType.ForeignKey:
                    if (loadDomainObjectAsForeignKey == null)
                    {
                        var associatedDomainObject = await LoadAssociatedDomainObject(
                            Domain.Associated<T>.LoadingType.Reference,
                            loadDomainObjectAsReference);
                        var reference = associatedDomainObject.Reference;
                        associatedDomainObject.Reference = null;
                        associatedDomainObject.ForeignKey = reference.Id;
                        return associatedDomainObject;
                    }
                    else
                    {
                        return loadDomainObjectAsForeignKey();
                    }
                case Domain.Associated<T>.LoadingType.Reference:
                    if (loadDomainObjectAsReference == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as reference because loader is null");
                    return await loadDomainObjectAsReference();
                default:
                    throw new InvalidOperationException(
                        $"Unknown value {desiredLoadingType} of {typeof(Domain.Associated<T>.LoadingType).Name}");
            }
        }

        protected async Task<IEnumerable<Domain.Associated<T>>> LoadAssociatedDomainObjectSet<T>(
            Domain.Associated<T>.LoadingType desiredLoadingType,
            Func<Task<IEnumerable<Domain.Associated<T>>>> loadDomainObjectAsReference,
            Func<Task<IEnumerable<Domain.Associated<T>>>> loadDomainObjectAsForeignKey)
            where T : Domain.DomainObjectBase
        {
            switch (desiredLoadingType)
            {
                case Domain.Associated<T>.LoadingType.None:
                    return null;
                case Domain.Associated<T>.LoadingType.ForeignKey:
                    if (loadDomainObjectAsForeignKey == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as foreign-key because loader is null");
                    return await loadDomainObjectAsForeignKey();
                case Domain.Associated<T>.LoadingType.Reference:
                    if (loadDomainObjectAsReference == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as reference because loader is null");
                    return await loadDomainObjectAsReference();
                default:
                    throw new InvalidOperationException(
                        $"Unknown value {desiredLoadingType} of {typeof(Domain.Associated<T>.LoadingType).Name}");
            }
        }
    }
}
