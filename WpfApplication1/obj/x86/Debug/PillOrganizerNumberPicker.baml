   M S B A M L   `   `   `  ����  WpfApplication1/    )WpfApplication1.PillOrganizerNumberPicker   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          � 	Resources  5	       �� 
TargetType   SquareButton �� Property �� Value �� RelativeSource��%�   &	        �� 5
   
   " ��6   .�5      ��  � �� 6)   �� *ActualHeight+ �� *Self+�� 5       � �� 6*   �� *ActualWidth+ �� *Self+5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��1* �6    � ColumnDefinitions 5   
   �� 5      $	��.4* �6   �� 5      $	��.1* �6   �� 5      $	��.1* �6   �� 5      $	��.1* �6   .+�5   
   (� $��0��6   $��0��6    (� Stretch$ Uniform=�6/   $��Center=�6A   .��5      �� -   5         ��Name$ medLabel��5      $��5q�6A   $��
[med-name]��6L   5      �� 5   
   -   5        	 ��Name$	 cmdMinus��5   q   $��0��6   $��1��6   $��Center=�6.   #��[  6K   $��5q�6�   ��5      (� 5      $ Uniform=�6   .��5       �� $
��Bold"�6   .��6.   -61   5!      5#      (� 5$   
   $��0��6   $��2��6   $��Center=�6/   $
 Fill=�6L   .��5%   
   �� -   5        
 ��Name$
 txtNum��5%   /   $��0��6   $��2��6   $��	10,5,10,5q�6Y   $��0��6l   $
��Auto��6u   $
��Auto��6�   6�   5'      �� 5(   
   -   5        $	 cmdPlus��5(   q   $��0��6   $��3��6   $��Center=�6.   #��[  6K   $��5q�6�   ��5)      (� 5*      $ Uniform=�6   .��5+      �� $
��Bold"�6   .��6.   +61   5,      5.      5/      50      