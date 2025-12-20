import { FormGroup, ValidatorFn } from "@angular/forms";

export function applyValidators(
  form: FormGroup,
  controls: string[],
  validators: ValidatorFn[]
) {
  controls.forEach(c => form.get(c)?.setValidators(validators));
  controls.forEach(c => form.get(c)?.updateValueAndValidity({ emitEvent: false }));
}

export function clearValidators(form: FormGroup, controls: string[]) {
  controls.forEach(c => form.get(c)?.clearValidators());
  controls.forEach(c => form.get(c)?.updateValueAndValidity({ emitEvent: false }));
}
export function getToday(): string {
  const d = new Date();
  const yyyy = d.getFullYear();
  const mm = ('0' + (d.getMonth() + 1)).slice(-2);
  const dd = ('0' + d.getDate()).slice(-2);
  return `${yyyy}-${mm}-${dd}`;
}
