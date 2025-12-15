import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appDecimalNumbers]',
  standalone: false,
})
export class DecimalNumbersDirective {
  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    const input = event.target as HTMLInputElement;
    const char = event.key;

    // allow digits
    if (/^[0-9]$/.test(char)) return;

    // allow only one dot
    if (char === '.' && !input.value.includes('.')) return;

    event.preventDefault();
  }
}
