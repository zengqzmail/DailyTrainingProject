   M S B A M L   `   `   `  ����  WpfApplication1     WpfApplication1.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Doctor��5   	   $	��350��6   $	��525��6%    � WindowState$ 	Maximized=�61    � 
ResizeMode$ NoResize=�6I   $	��735��6_    � SizeToContent$ Manual=�6n   |�.  � Topmost .  � WindowStyle$
 None=�6�   $��40(�6�    � 	Resources 5       �� 
TargetType �� Triggers	 P� Property
 P� Value �� Property �� Value��%�  '��       '	� �     �� 5   
   " ��6    5      P� 5	      	 �@d�	IsChecked$

 trueY�6+   . �5
      ��  � ��
 ��  �63   �� 5       � ��$
 Bold"�63   5      5      �� 5   
   " 	�6    5   
   P� 5      	 �@d�	IsChecked$

 trueY�6+   . �5      ��  � ��
 ��  �6/   �� 5       � ��$
 Bold"�6/   5      5      5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.Close��5      5       J� InputBindings 5      �� 5   
    �� Command$ ApplicationCommands.Close��6    �� Gesture$ Ctrl+Shift+X��5      .��5&      � -   5        $��grid1��5&      $
��Auto��6   $
��Auto��6&    � UseLayoutRounding . |�.  � RowDefinitions 5'   
   �� 5(      $
��117* �6   �� 5)      $	��10* �6   �� 5*      $	��28* �6   �� 5+      $	��16* �6   �� 5,      $
��130* �6   �� 5-      $	��10* �6    � ColumnDefinitions 5/   
   �� 50      $��6* �6   �� 51      $
��122* �6   �� 52      $
��268* �6   �� 53      $	��51* �6   �� 54      $
��103* �6   �� 55      $	��96* �6   �� 56      $	��48* �6   �� 57      $	��12* �6   �� 58      $��7* �6   .+�5:   
   �� -   5        $��
textBlock1��5:   S   $
��Auto��6   $��Stretch=�6"   $��	26,12,0,0q�6@   $����  When you are ready to begin, please select English or Spanish by pressing the appropriate button below. 
 
   Cuando esté listo para comenzar, por favor seleccione Ingles o Español oprimiendo el botón apropiado.��6e   $	��Top=�6L  $��7��6d  $��WrapWithOverflow=�6x  $��32(�6�  $��1��6�  $��6��6�  �� 5;   
   -   5        $��button1��5;   K   $��English��6   $	��156��6#   $
��Left=�60   $��Bottom=�6Z   $	��337��6u   $��3��6�   $��5��6�   $��40(�6�   $��2��6�   $��	44,0,0,83q�6�   6�   �� 5<   
   -   5        $��button2��5<   \   $��Español��6   $��2��6#   $	��156��63   $��Right=�6@   $��Bottom=�6k   $	��337��6�   $��4��6�   $��	0,0,81,83q�6�   $��5��6�   $��1q�6�   6�   5=      5>      