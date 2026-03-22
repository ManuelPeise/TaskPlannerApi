import React from "react";

const useForm = <TModel>(
  initialValues?: TModel,
  submitCallback?: (model: TModel) => void | Promise<void>,
) => {
  const [model, setModel] = React.useState<TModel>(
    initialValues ?? ({} as TModel),
  );

  const submitCallbackRef = React.useRef(submitCallback);

  const handleChange = (key: keyof TModel, value: any) => {
    setModel((prevModel) => ({
      ...prevModel,
      [key]: value,
    }));
  };

  const handleSubmit = async () => {
    if (submitCallbackRef.current) {
      await submitCallbackRef.current(model);
    }
  };

  const subscribe = React.useCallback(() => {
    return { ...model };
  }, [model]);

  const subscribeProperty = React.useCallback(
    (key: keyof TModel) => {
      return model[key];
    },
    [model],
  );

  const resetForm = React.useCallback(() => {
    setModel(initialValues ?? ({} as TModel));
  }, [initialValues]);

  const isModified = React.useMemo(() => {
    return JSON.stringify(model) !== JSON.stringify(initialValues);
  }, [model, initialValues]);

  return {
    model,
    isModified,
    handleChange,
    handleSubmit,
    subscribe,
    subscribeProperty,
    resetForm,
  };
};

export default useForm;
