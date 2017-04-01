import Validation, { validate } from "billing-ui/helpers/ValidationHelpers";

const ValidationFields = {
    name: Validation.Required(),
    text: Validation.Required()
};

export const validationFieldLabels = {};

export const getValidationResult = form => {
    return (Object.keys(ValidationFields || {})).reduce((result, field) => {
        result[field] = validate(form[field], ValidationFields[field]);
        return result;
    }, {});
};

export default ValidationFields;
