import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appNumbersOnly]',
  standalone: false,
})
export class NumbersOnlyDirective {
  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    const char = event.key;

    // Allow digits 0-9 only
    if (!/^[0-9]$/.test(char)) {
      event.preventDefault();
    }
  }
}
